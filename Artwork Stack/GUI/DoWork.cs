using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Artwork_Stack.Controls;
using Artwork_Stack.DataAccessLayer;
using Artwork_Stack.Model;
using Artwork_Stack.Tools;
using TagLib;

/* 
 * NOTE: There is awkward issue with cyrillic tags in taglib,
 * id3 specs allows an iso-8859 encoding, but taglib makes this tags unreadable.
 * Since it hard to estimate readable text or not, I've just add button to manual correction.
*/

// TODO: check crop/resample features
// TODO: xml special chars decode; 
// TODO: hotkeys - next-prev-skip

namespace Artwork_Stack.GUI
{
    public partial class DoWork : Form
    {
        private readonly JobController jCon;
        private DataRow currentJob;
        
        private readonly Label generalmsg = new Label
        {
            BackColor = Color.FromArgb(108, 140, 213),
            Padding = new Padding(4),
            TextAlign = ContentAlignment.MiddleCenter,
            AutoSize = true,
            MinimumSize = new Size(200, 15),
            MaximumSize = new Size(200, 300),
            Visible = false
        };

        public DoWork(JobController jcon)
        {
            InitializeComponent();
            jCon = jcon;
            foreach (var c in Services.Providers)
            {
                Sources.TabPages.Add(
                    new TabPage
                    {
                        Name = Enum.GetName(typeof(Services.Supported), c.Value.InternalID),
                        Text = c.Value.DisplayedName
                    }
                );
            }
        }

        private void formDoWork_Shown(object sender, EventArgs e)
        {
            this.Controls.Add(generalmsg);
            generalmsg.MouseClick += HideEventHandler;

            cellEmbeded = new imageCell(120, 150, 840, 280);
            cellEmbeded.Caption = Verbal.Embedded;
            cellEmbeded.IsEmbeded = true;
            cellEmbeded.Click += CellClick;
            this.Controls.Add(cellEmbeded);

            currentJob = jCon.Jobs.Tables[Fields.Tracks].Rows[0];

            bindGrid();
            if (WinRegistry.GetValue<bool>(WinRegistry.Keys.ShowJobs))
            {
                btnJobs.Checked = true;
            }
            else
            {
                btnJobs_CheckedChanged(null, null);
            }

            chkCrop.Checked   = WinRegistry.GetValue<bool>(WinRegistry.Keys.Crop);
            chkResize.Checked = WinRegistry.GetValue<bool>(WinRegistry.Keys.Resize);
            
            int size = WinRegistry.GetValue<int>(WinRegistry.Keys.ResizeTo);
            numSize.Value = size > 0 ? size : 500;

            showTrackInfo();
            DoSearch();
        }

        private ServiceContext GetContextByTab(TabPage tab)
        {
            var currentprovier = (Services.Supported)Enum.Parse(typeof(Services.Supported), tab.Name);
            return Services.Providers[currentprovier];
        }

        private ServiceContext GetCurrentContext()
        {
            return GetContextByTab(Sources.SelectedTab);
        }

        private void ShowResults(List<UnifiedResponse> Responses)
        {
            for (int c = 0; c < Responses.Count(); c++)
            {
                var Response = Responses[c];
                var tab = Sources.TabPages[c];
                var context = GetContextByTab(tab);
                int count = Math.Min(20, Response.Results.Count);

                tab.Text = string.Format("{0} ({1})", context.DisplayedName, count);

                if (count == 0)
                {
                    continue;
                }

                int cursor = 0;
                int i = 0;

                while (cursor < Response.Results.Count && i < count)
                {
                    var r = Response.Results[cursor];
                    if (string.IsNullOrEmpty(r.Url) && string.IsNullOrEmpty(r.Thumb))
                    {
                        cursor++;
                        continue;
                    }

                    var cell = new imageCell(120, 150, 120 * (i % 5), 150 * (i / 5));
                    cell.Caption = string.Format(
                        "{0}; {1}",
                        r.Album,
                        r.Width > 0 && r.Height > 0 ? string.Format("{0}x{1}px", r.Width, r.Height) : "size\u00A0n/a"
                    );

                    Label l = null;
                    cell.MouseEnter += (sender, args) => l = ShowInfoTooltip(cell.Caption, cell.Location + new Size(16, cell.Height));
                    cell.MouseLeave += (sender, args) => { this.Controls.Remove(l); l.Dispose(); };

                    cell.Click += CellClick;
                    cell.FullSizeUrl = r.Url;
                    if (context.Provider.GetFullsizeUrlViaCallback)
                    {
                        cell.AdditionalInfo = r.AdditionalInfo;
                    }
                    tab.Controls.Add(cell);
                    (new Thread(() => cell.Thumbnail = Http.getPicture(r.Thumb))).Start();

                    cursor++;
                    i++;
                }
            }
        }

        private void SearchWorker(string Query, out List<UnifiedResponse> Response)
        {
            var ThreadPool = new List<Thread>();
            var ContextPool = Services.Providers.Values.ToArray();
            var Results = new UnifiedResponse[ContextPool.Length];

            for (int i = 0; i < ContextPool.Length; i++)
            {
                int closure = i;
                Results[i] = new UnifiedResponse();
                var thread = new Thread(() => Results[closure] = ContextPool[closure].Provider.Search(Query));
                thread.Start();
                ThreadPool.Add(thread);
            }

            while(ThreadPool.Any(t => t.IsAlive))
            {
                Application.DoEvents();
            }

            Response = Results.ToList();
        }

        private void DoSearchPreinit()
        {
            foreach (TabPage page in Sources.TabPages)
            {
                page.Controls.Clear();
                page.Text = GetContextByTab(page).DisplayedName;
            }
        }

        private void DoSearch()
        {
            DoSearchPreinit();
            HideEventHandler(null, null);

            ShowBusy();
            List<UnifiedResponse> Response = null;
            SearchWorker(txtQuery.Text, out Response);
            HideBusy();

            if (Response.All(r => r.ResultsCount == 0))
            {
                ShowGeneralMessage(Verbal.NoResults);
            }
            else
            {
                ShowResults(Response);
            }
        }

        private void btnOverrideSearch_Click(object sender, EventArgs e)
        {
            if (sender == null) return;
            DoSearch();
        }

        private void CellClick(object _sender, EventArgs _e)
        {
            var e    = (MouseEventArgs)_e;
            var cell = (imageCell)((clickHandler)_sender).Parent;

            if (e.Button == MouseButtons.Left)
            {
                ShowBusy();
                var image = GetFullImage(cell);
                if (image != null)
                {
                    var viewer = new ShowFull(image, chkCrop.Checked);
                    viewer.ShowDialog();
                    if (viewer.NotAvailable)
                    {
                        cell.Thumbnail = Properties.Resources.noartwork;
                    }
                    if (viewer.Selected)
                    {
                        unselectCells();
                        cell.Check();
                    }
                }
                else
                {
                    ShowTipMessage(Verbal.PicNotAvailable);
                    cell.Thumbnail = Properties.Resources.noartwork;
                }
                HideBusy();
            }
            else if (e.Button == MouseButtons.Right)
            {
                bool meChecked = cell.Checked;
                unselectCells();
                if (meChecked) cell.UnCheck(); else cell.Check();
            }
        }

        private Image GetFullImage(imageCell cell)
        {
            Image img = null;

            if (cell.FullSize != null)
            {
                img = cell.FullSize;
            }
            else
            {
                var provider = GetCurrentContext().Provider;
                string url = string.Empty;
                if (!string.IsNullOrEmpty(cell.FullSizeUrl))
                {
                    url = cell.FullSizeUrl;
                }
                else if (provider.GetFullsizeUrlViaCallback)
                {
                    var tgeturl = new Thread(() => url = provider.GetFullsizeUrlCallback(cell.AdditionalInfo));
                    tgeturl.Start();
                    while (tgeturl.IsAlive)
                    {
                        Application.DoEvents();
                    }
                }

                var tget = new Thread(() => img = Http.getPicture(url));
                tget.Start();
                while (tget.IsAlive)
                {
                    Application.DoEvents();
                }

                cell.FullSize = img;
            }

            return img;
        }

        private void buttonCycleClick(object sender, EventArgs e)
        {
            DataRow stored = currentJob;
            int curJobIndex = (int)currentJob[Fields.ID];

            int startIndex, endIndex;
            bool isForwardClick = ((Button)sender).Name == "btnNext";
            if (isForwardClick)
            {
                startIndex = curJobIndex == jCon.TopMargin ? curJobIndex : (curJobIndex + 1);
                endIndex   = jCon.JobsCount;
            }
            else
            {
                startIndex = curJobIndex - 1;
                endIndex   = 0;
            }

            int i = startIndex;
            while (true)
            {
                if (!jCon.IsUnprocessedJobs) break;
                DataRow newdr = jCon.Jobs.Tables[Fields.Tracks].Rows[i];
                if ((bool)newdr[Fields.Done] || (bool)newdr[Fields.Process])
                {
                    if ( isForwardClick && i != endIndex) { i++; continue; }
                    if (!isForwardClick && i != endIndex) { i--; continue; }
                }
                else
                {
                    currentJob = newdr; break;
                }
            }

            if (!chkSkip.Checked && !cellEmbeded.Checked)
            {
                stored[Fields.Process] = true;
                saveArtWork(stored, getCurrentArtwork());
            }
            else
            {
                if (stored[Fields.PathList].GetType() != typeof(DBNull))
                {
                    string msg = string.Format(Verbal.AlbumModeOmit, ((List<String>)stored[Fields.PathList]).Count);
                    if (MessageBox.Show(msg, "", MessageBoxButtons.OKCancel) == DialogResult.Cancel) return;
                }
                jCon.SetJobIsDone(curJobIndex);
                unselectCells();
                chkSkip.Checked = false;
            }

            if (!jCon.IsUnprocessedJobs)
            {
                btnPrev.Enabled = btnNext.Enabled = false;
                MessageBox.Show(Verbal.Alldone);
            }
            else
            {
                btnPrev.Enabled = (int) currentJob[Fields.ID] >  jCon.BottomMargin;
                btnNext.Enabled = (int) currentJob[Fields.ID] <= jCon.TopMargin;
                showTrackInfo();
                txtQuery.Text = currentJob[Fields.Artist] + @" " + currentJob[Fields.Album];
                DoSearch();
            }
        }

        private void unselectCells()
        {
            foreach (var c in Sources.SelectedTab.Controls.OfType<imageCell>())
            {
                c.UnCheck();
            }
            cellEmbeded.UnCheck();
        }

        private Image getCurrentArtwork(bool uncheckAll = true)
        {
            var result = cellEmbeded.Checked ? cellEmbeded : Sources.SelectedTab.Controls.OfType<imageCell>().FirstOrDefault(c => c.Checked);
            if (uncheckAll) unselectCells();
            return result == null ? null : GetFullImage(result);
        }

        private void showTrackInfo()
        {
            bool groupMode = string.IsNullOrEmpty(currentJob[Fields.Path].ToString());
            string path = groupMode ? ((List<String>)currentJob[Fields.PathList])[0] : currentJob[Fields.Path].ToString();

            txtQuery.Text = jCon.CreateQueryString(int.Parse(currentJob[Fields.ID].ToString()));
            gridCurrentJob.Rows.Clear();
            gridCurrentJob.Rows.Add(Verbal.JobIndex, (int)currentJob[Fields.ID]);
            gridCurrentJob.Rows.Add(Verbal.DoneTotalJobs, jCon.CompletedJobsCount + "/" + jCon.JobsCount);
            gridCurrentJob.Rows.Add(Fields.Path,  groupMode ? Verbal.GroupingPath : path);
            gridCurrentJob.Rows.Add(Verbal.Track, groupMode ? Verbal.GroupingTitles : currentJob[Fields.Title].ToString());
            gridCurrentJob.Rows.Add(Fields.Artist, currentJob[Fields.Artist].ToString());
            gridCurrentJob.Rows.Add(Fields.Album, currentJob[Fields.Album].ToString());

            if (groupMode)
            {
                gridCurrentJob.Rows.Add(
                    Verbal.Warning, 
                    string.Format(Verbal.ArtAppliesCount, ((List<String>)currentJob[Fields.PathList]).Count)
                );
            }

            var track = TagLib.File.Create(path);
            gridCurrentJob.Rows.Add(Verbal.ArtInFile, track.Tag.Pictures.GetLength(0));
            if (track.Tag.Pictures.GetLength(0) > 0)
            {
                var ic = new ImageConverter();
                Image i = null;
                try
                {
                    cellEmbeded.Thumbnail = cellEmbeded.FullSize = i = (Image)ic.ConvertFrom(track.Tag.Pictures[0].Data.Data);
                }
                catch(ArgumentException e) { } // Supress strange mime-formats, that Image Converter can't handle
                if (i != null)
                {
                    cellEmbeded.Caption = string.Format(
                        "{0}; {1}x{2}px",
                        Verbal.Embedded,
                        cellEmbeded.FullSize.Width, 
                        cellEmbeded.FullSize.Height
                    );
                }
            }
            else
            {
                cellEmbeded.Thumbnail = Properties.Resources.noartwork;
                cellEmbeded.Caption = Verbal.NoEmbedded;
            }
            track.Dispose();
        }

        private void saveArtWork(DataRow job, Image artwork)
        {
            if (artwork == null)
            {
                job[Fields.Process] = false;
                return;
            }

            var pathLists = new List<string>();
            if (string.IsNullOrEmpty(job[Fields.Path].ToString())) pathLists = (List<String>)job[Fields.PathList];
            else pathLists.Add((string)job[Fields.Path]);

            (new Thread(() => saveArtWorkWorker(pathLists, artwork, (int)job[Fields.ID]))).Start();
        }

        private void saveArtWorkWorker(IEnumerable<string> files, Image artwork, int jobID)
        {
            if (artwork == null || artwork.Width == 0 || artwork.Height == 0)
            {
                jCon.Jobs.Tables[Fields.Tracks].Rows[jobID][Fields.Process] = false;
                return;
            }

            if (chkResize.Checked && (artwork.Width > numSize.Value || artwork.Height > numSize.Value))
            {
                artwork = Images.ResizeImage(artwork, new Size((int)numSize.Value, (int)numSize.Value));
            }

            if (chkCrop.Checked) artwork = Images.CropImage(artwork);

            var stream = new MemoryStream();
            artwork.Save(stream, ImageFormat.Jpeg);
            var buffer = new ByteVector(stream.ToArray());
            var pic = new Picture(buffer);
            Picture[] artworkFrame = { pic };
            foreach (var file in files)
            {
                var track = TagLib.File.Create(file);
                track.Tag.Pictures = artworkFrame;
                track.Save();
            }
            jCon.SetJobIsDone(jobID);
        }

        public void Navigate(int jobId)
        {
            var job = jCon.Jobs.Tables[Fields.Tracks].Rows[jobId];
            if (!(bool)job[Fields.Done] && !(bool)job[Fields.Process])
            {
                currentJob = job;
                showTrackInfo();
                txtQuery.Text = currentJob[Fields.Artist] + @" " + currentJob[Fields.Album];
                DoSearch();
                chkSkip.Checked = false;
                btnPrev.Enabled = (int)currentJob[Fields.ID] > jCon.BottomMargin;
                btnNext.Enabled = (int)currentJob[Fields.ID] <= jCon.TopMargin;
            }
        }

        private void DoWork_FormClosing(object sender, FormClosingEventArgs e)
        {
            ShowBusy();
            while(jCon.ProcessedJobsCount > 0)
            {
                Application.DoEvents();
            }
        }

        private void ShowBusy()
        {
            Busy.Show();
        }

        private void HideBusy()
        {
            Busy.Hide();
        }

        private void gridJobs_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            Navigate((int)gridJobs.SelectedRows[0].Cells[Fields.ID].Value);
        }

        private void btnJobs_CheckedChanged(object sender, EventArgs e)
        {
            WinRegistry.SetValue(WinRegistry.Keys.ShowJobs, btnJobs.Checked);
            this.Height = btnJobs.Checked ? 910 : 678;
        }

        private void bindGrid()
        {
            gridJobs.DataSource = jCon.Jobs.Tables[Fields.Tracks];
            // ReSharper disable PossibleNullReferenceException
            gridJobs.Columns[Fields.Path].Visible = false;
            gridJobs.Columns[Fields.ID].Width = 40;
            gridJobs.Columns[Fields.Artist].Width = 265;
            gridJobs.Columns[Fields.Title].Width = 270;
            gridJobs.Columns[Fields.Album].Width = 270;
            gridJobs.Columns[Fields.Done].Width = 40;
            gridJobs.Columns[Fields.Process].Width = 60;
            // ReSharper restore PossibleNullReferenceException
            foreach (DataGridViewRow row in gridJobs.Rows)
            {
                if (string.IsNullOrEmpty(row.Cells[Fields.Path].Value.ToString()))
                {
                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        cell.Style.BackColor = row.Index % 2 == 0 ? Color.FromArgb(240, 210, 240) : Color.FromArgb(255, 220, 255);
                    }
                }
            }
        }

        private Label ShowInfoTooltip(string msg, Point location)
        {
            var l = new Label
            {
                Text = msg,
                Location = location,
                BackColor = Color.FromArgb(255, 246, 115),
                Padding = new Padding(4),
                TextAlign = ContentAlignment.MiddleCenter,
                AutoSize = true,
                MinimumSize = new Size(120, 15),
                MaximumSize = new Size(120, 300)
            };
            this.Controls.Add(l);
            l.BringToFront();
            return l;
        }

        private void ShowGeneralMessage(string msg)
        {
            generalmsg.Text = msg;
            generalmsg.Location = new Point((Sources.Width - generalmsg.Width) / 2, (Sources.Height - generalmsg.Height) / 2);
            generalmsg.Visible = true;
            generalmsg.BringToFront();
        }

        private void ShowTipMessage(string msg)
        {
            ShowGeneralMessage(msg);

            var s = new System.Windows.Forms.Timer();
            s.Interval = 5000;
            s.Start();
            s.Tick += (sender, args) => { generalmsg.Visible = false; s.Stop(); s.Dispose(); };
        }

        private void HideEventHandler(object sender, EventArgs e)
        {
            generalmsg.Visible = false;
        }

        private void btnEncode_Click(object sender, EventArgs e)
        {
            currentJob[Fields.Artist] = currentJob[Fields.Artist].ToString().Correct();
            currentJob[Fields.Album]  = currentJob[Fields.Album ].ToString().Correct();
            currentJob[Fields.Title]  = currentJob[Fields.Title ].ToString().Correct();
            txtQuery.Text = txtQuery.Text.Correct();
            showTrackInfo();
            DoSearch();
        }

        private void chkCrop_CheckedChanged(object sender, EventArgs e)
        {
            WinRegistry.SetValue(WinRegistry.Keys.Crop, chkCrop.Checked);
        }

        private void chkResize_CheckedChanged(object sender, EventArgs e)
        {
            WinRegistry.SetValue(WinRegistry.Keys.Resize, chkResize.Checked);
        }

        private void numSize_ValueChanged(object sender, EventArgs e)
        {
            WinRegistry.SetValue(WinRegistry.Keys.ResizeTo, numSize.Value);
        }
    }
}
