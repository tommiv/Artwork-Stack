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

// TODO: save interface settings to reg; check crop/resample features; check cyrillic; replace messagebox by graphic msgs

namespace Artwork_Stack.GUI
{
    public partial class DoWork : Form
    {
        private readonly JobController jCon;
        private DataRow currentJob;

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
            cellEmbeded = new imageCell(120, 150, 840, 280);
            cellEmbeded.Caption = @"Embeded art";
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

            showTrackInfo();
            DoSearch();
        }

        private ServiceContext GetCurrentContext()
        {
            var currentprovier = (Services.Supported)Enum.Parse(typeof(Services.Supported), Sources.SelectedTab.Name);
            return Services.Providers[currentprovier];
        }

        private void ShowResults(UnifiedResponse Response)
        {
            HideBusy();

            Sources.SelectedTab.Controls.Clear();

            int cursor = 0;
            int i = 0;
            while (cursor < Response.Results.Count && i < Math.Min(20, Response.Results.Count))
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
                cell.Click += CellClick;
                cell.FullSizeUrl = r.Url;
                if (GetCurrentContext().Provider.GetFullsizeUrlViaCallback)
                {
                    cell.AdditionalInfo = r.AdditionalInfo;
                }
                Sources.SelectedTab.Controls.Add(cell);
                (new Thread(() => cell.Thumbnail = Http.getPicture(r.Thumb))).Start();

                cursor++;
                i++;
            }
        }

        private static void SearchWorker(ServiceContext SCon, string Query, out UnifiedResponse Response)
        {
            Response = SCon.Provider.Search(Query);
            if (Response.ResultsCount == 0)
            {
                MessageBox.Show(@"No results");
            }
        }

        private void DoSearch()
        {
            showBusy();

            UnifiedResponse Response = null;
            
            var context = GetCurrentContext();
            string query = txtQuery.Text;

            var t = new Thread(() => SearchWorker(context, query, out Response));
            t.Start();
            while (t.IsAlive)
            {
                Application.DoEvents();
            }
            ShowResults(Response);
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
                showBusy();
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
                    MessageBox.Show("Picture not available");
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
                    string msg = string.Format(
                        "This unit is album-group. If you check 'skip' or 'embedded art', {0} files will be skipped ",
                        ((List<String>)stored[Fields.PathList]).Count
                    );
                    if (MessageBox.Show(msg, "", MessageBoxButtons.OKCancel) == DialogResult.Cancel) return;
                }
                jCon.SetJobIsDone(curJobIndex);
                unselectCells();
                chkSkip.Checked = false;
            }

            if (!jCon.IsUnprocessedJobs)
            {
                btnPrev.Enabled = btnNext.Enabled = false;
                MessageBox.Show(@"That's all, folks!");
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
            gridCurrentJob.Rows.Add("Job index", (int)currentJob[Fields.ID]);
            gridCurrentJob.Rows.Add("Done/total jobs", jCon.CompletedJobsCount + "/" + jCon.JobsCount);
            gridCurrentJob.Rows.Add(Fields.Path, groupMode ? "Grouping mode, multiple paths" : path);
            gridCurrentJob.Rows.Add("Track", groupMode ? "Grouping mode, multiple titles" : currentJob[Fields.Title].ToString());
            gridCurrentJob.Rows.Add(Fields.Artist, currentJob[Fields.Artist].ToString());
            gridCurrentJob.Rows.Add(Fields.Album, currentJob[Fields.Album].ToString());

            if (groupMode) gridCurrentJob.Rows.Add("WARN", string.Format("Art applies (or not) to {0} files!", ((List<String>)currentJob[Fields.PathList]).Count));

            var track = TagLib.File.Create(path);
            gridCurrentJob.Rows.Add("Art in file", track.Tag.Pictures.GetLength(0));
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
                        "Embedded Art; {0}x{1}px", 
                        cellEmbeded.FullSize.Width, 
                        cellEmbeded.FullSize.Height
                    );
                }
            }
            else
            {
                cellEmbeded.Thumbnail = Properties.Resources.noartwork;
                cellEmbeded.Caption = @"No Embedded Art";
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
            showBusy();
            while(jCon.ProcessedJobsCount > 0)
            {
                Application.DoEvents();
            }
        }

        private void Sources_SelectedIndexChanged(object sender, EventArgs e)
        {
            DoSearch();
        }

        private void showBusy()
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
                        cell.Style.BackColor =
                            row.Index % 2 == 0
                            ? Color.FromArgb(255, 240, 210, 240)
                            : Color.FromArgb(255, 255, 220, 255);
                    }
                }
            }
        }
    }
}
