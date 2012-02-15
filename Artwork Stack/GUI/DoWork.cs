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

namespace Artwork_Stack.GUI
{
    public partial class DoWork : Form
    {
        private readonly JobController jCon;

        private DataRow   currentJob;
        private imageCell cellEmbeded;
        private Image     EmbededArt;
        private Jobs      fJobs;

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
            picBusy.Location = new Point(0, 0);
            picBusy.Size = this.Size;
            picBusy.BringToFront();

            var t = new Thread(() => jCon.TraverseFolder());
            t.Start();
            while (t.IsAlive) Application.DoEvents();

            if (jCon.JobsCount <= 0)
            {
                MessageBox.Show(@"Folders have no mp3 files or all files were filtered out");
                picBusy.Visible = false;
                foreach (Control c in this.Controls) c.Enabled = false;
                return;
            }

            currentJob = jCon.Jobs.Tables[Fields.Tracks].Rows[0];
            if (WinRegistry.GetValue<bool>(WinRegistry.Keys.ShowJobs))
            {
                btnJobs.Checked = true;
            }

            cellEmbeded = new imageCell(120, 150, 840, 280);
            cellEmbeded.Text = @"embeded";
            cellEmbeded.Click += CellClick;
            Controls.Add(cellEmbeded);

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
            picBusy.Hide();

            Sources.SelectedTab.Controls.Clear();

            int cursor = 0;
            int i = 0;
            while (cursor < Response.Results.Count && i < Math.Min(20, Response.Results.Count))
            {
                var r = Response.Results[cursor];
                if (string.IsNullOrEmpty(r.Url))
                {
                    cursor++;
                    continue;
                }

                var cell = new imageCell(120, 150, 120 * (i % 5), 150 * (i / 5));
                cell.Caption = string.Format("{0}; {1}x{2}px", r.Album, r.Width, r.Height);
                cell.Click += CellClick;
                cell.ClickHandler.Storage = r.Url;
                Sources.SelectedTab.Controls.Add(cell);
                (new Thread(() => cell.Image = Http.getPicture(r.Url))).Start();

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
                return;
            }
        }

        private void DoSearch()
        {
            picBusy.Show();

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
            var e      = (MouseEventArgs)_e;
            var sender = (clickHandler)_sender;
            var p      = (imageCell)sender.Parent;

            if (e.Button == MouseButtons.Left)
            {
                if (p.Text == @"embeded" || !string.IsNullOrEmpty(sender.Storage)) // todo move embed to const
                {
                    var viewer = p.Text == @"embeded"? new ShowFull(EmbededArt) : new ShowFull(sender.Storage, chkCrop.Checked);
                    viewer.ShowDialog();
                    if (viewer.NotAvailable) ((imageCell)sender.Parent).Image = Properties.Resources.noartwork;
                    if (viewer.Selected)
                    {
                        unselectCells();
                        ((imageCell)sender.Parent).Check();
                    }

                }
            }
            else if (e.Button == MouseButtons.Right)
            {
                foreach (var c in p.Parent.Controls.OfType<imageCell>().Where(c => c != p))
                {
                    c.UnCheck();
                }
                if (p.Checked) p.UnCheck(); else p.Check();
            }
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
                saveArtWork(stored, getCurrentArtworkUrl());
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

        private void unselectCells() { foreach (var c in Controls.OfType<imageCell>()) { c.UnCheck(); } }

        private string getCurrentArtworkUrl(bool uncheckAll = true)
        {
            var result = Sources.SelectedTab.Controls.OfType<imageCell>().FirstOrDefault(c => c.Checked);
            if (uncheckAll) unselectCells();
            return result == null ? string.Empty : result.ClickHandler.Storage;
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
                try
                {
                    cellEmbeded.Image = EmbededArt = (Image)ic.ConvertFrom(track.Tag.Pictures[0].Data.Data);
                }
                catch(ArgumentException e) { } // Supress strange mime-formats, that Image Converter can't handle
                cellEmbeded.Caption = @"Embedded Art";
            }
            else
            {
                cellEmbeded.Image = Properties.Resources.noartwork;
                EmbededArt = null;
                cellEmbeded.Caption = @"No Embedded Art";
            }
            track.Dispose();
        }

        private void saveArtWork(DataRow job, string artworkUrl)
        {
            if (String.IsNullOrEmpty(artworkUrl))
            {
                job[Fields.Process] = false;
                return;
            }

            var pathLists = new List<string>();
            if (string.IsNullOrEmpty(job[Fields.Path].ToString())) pathLists = (List<String>)job[Fields.PathList];
            else pathLists.Add((string)job[Fields.Path]);

            (new Thread(() => saveArtWorkWorker(pathLists, artworkUrl, (int)job[Fields.ID]))).Start();
        }

        private void saveArtWorkWorker(IEnumerable<string> files, string artworkURL, int jobID)
        {
            var artwork = Http.getPicture(artworkURL);
            if (artwork == null || artwork.Width == 0 || artwork.Height == 0)
            {
                jCon.Jobs.Tables[Fields.Tracks].Rows[jobID][Fields.Process] = false;
                return;
            }

            if (chkResize.Checked && (artwork.Width > numSize.Value || artwork.Height > numSize.Value))
                artwork = Images.ResizeImage(artwork, new Size((int)numSize.Value, (int)numSize.Value));

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

        private void btnJobs_CheckedChanged(object sender, EventArgs e)
        {
            WinRegistry.SetValue(WinRegistry.Keys.ShowJobs, btnJobs.Checked);
            if (btnJobs.Checked)
            {
                fJobs = jCon.ShowJobList();
                fJobs.Visible = true;
                fJobs.Closed += (sndr, evargs) => btnJobs.Checked = false;
                stickJobs();
            }
            else
            {
                if (fJobs != null && !fJobs.IsDisposed)
                {
                    fJobs.Visible = false;
                    this.Focus();
                }
            }
        }

        private void stickJobs()
        {
            if (fJobs == null || fJobs.IsDisposed) return;
            fJobs.Size     = new Size(this.Width, fJobs.Height);
            fJobs.Location = new Point(this.Location.X, this.Location.Y + this.Height + 6);
        }

        private void formDoWork_LocationChanged(object sender, EventArgs e)
        {
            if (fJobs == null || fJobs.IsDisposed) return;
            stickJobs();
            fJobs.Focus();
            this.Focus();
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
    }
}
