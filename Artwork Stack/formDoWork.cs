using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Windows.Forms;
using System.Web.Script.Serialization;
using System.Threading;
using System.Drawing;
using ImageCell;
using TagLib;

using grid = Artwork_Stack.Model.grid;
using gImgAPIWorkerParams = Artwork_Stack.Model.gImgAPIWorkerParams;
using getIMGWorkerParams  = Artwork_Stack.Model.getIMGWorkerParams;

namespace Artwork_Stack
{
    public partial class formDoWork : Form
    {
        private readonly JobController jCon;
        private          DataRow       currentJob;
        private readonly imageCell[,]  cell = new imageCell[grid.i, grid.j];
        private          imageCell     cellEmbeded;
        private Image    EmbededArt;
        private formJobs fJobs;

        public formDoWork(JobController jcon)
        {
            InitializeComponent();
            jCon = jcon;
        }

        private void formDoWork_Shown(object sender, EventArgs e)
        {
            picBusy.Location = new Point(0,0);
            picBusy.Size     = this.Size;
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

            for (int ii = 0; ii < grid.i; ii++)
                for (int jj = 0; jj < grid.j; jj++)
                {
                    cell[ii, jj] = new imageCell(grid.w, grid.h, grid.lm + ii * grid.w + ii * grid.pw, grid.tm + jj * grid.h + jj * grid.ph);
                    cell[ii, jj].Click += CellClick;
                    Controls.Add(cell[ii, jj]);
                }

            cellEmbeded = new imageCell(100, 130, 724, 300);
            cellEmbeded.Text = @"embeded";
            cellEmbeded.Click += CellClick;
            Controls.Add(cellEmbeded);
            showTrackInfo();
            (new Thread(()=>googleIt(txtQuery.Text, picBusy))).Start();
        }

        private void gImgAPIWorker(Object P)
        {
            var p = (gImgAPIWorkerParams)P;
            var req = WebRequest.Create("https://ajax.googleapis.com/ajax/services/search/images?v=1.0&rsz=" + p.rsz + "&start=" + p.i * p.rsz + "&q=" + p.query);
            Stream resp = req.GetResponse().GetResponseStream();
            var reader = new StreamReader(resp);
            var jsonParser = new JavaScriptSerializer();
            dynamic gImgAPIResult = jsonParser.DeserializeObject(reader.ReadToEnd());
            if (gImgAPIResult["responseData"] == null || gImgAPIResult["responseData"]["results"].Length == 0) return;
            gImgAPIResult = gImgAPIResult["responseData"]["results"];
            var thread = new Thread[p.rsz];
            for (int j = 0; j < p.rsz; j++)
            {
                string caption = string.Format(
                    "{0}x{1}: {2}\n{3}",
                    gImgAPIResult[j]["width"],
                    gImgAPIResult[j]["height"],
                    gImgAPIResult[j]["url"].Substring(gImgAPIResult[j]["url"].LastIndexOf('.') + 1, 3),
                    gImgAPIResult[j]["visibleUrl"]
                );
                var parameters = new getIMGWorkerParams(gImgAPIResult[j]["tbUrl"], gImgAPIResult[j]["url"], p.i, j, 100, 100, caption);
                thread[j] = new Thread(getIMGWorker);
                thread[j].Start(parameters);
            }
        }

        private void getIMGWorker(Object P)
        {
            var p = (getIMGWorkerParams)P;
            Image thumb = httpRequest.getPicture(p.tburl);
            this.Invoke((Action)(() =>
            {
                cell[p.ix, p.iy].Image                = thumb;
                cell[p.ix, p.iy].Caption              = p.caption;
                cell[p.ix, p.iy].ClickHandler.storage = p.url;
            }
            ));
        }

        private void googleIt(string query, PictureBox busy = null)
        {
            foreach (var c in cell) c.Image = Properties.Resources.noartwork;
            var thread = new Thread[4];
            for (int i = 0; i < 4; i++)
            {
                var parameters = new gImgAPIWorkerParams(query, 4, i);
                thread[i] = new Thread(gImgAPIWorker);
                thread[i].Start(parameters);
            }

            if (busy == null) return;
            while (true)
            {
                bool process = false;
                foreach (var t in thread) process = process || t.IsAlive;
                if (!process) break;
                Application.DoEvents();
            }
            this.Invoke((Action)(() => busy.Visible = false));
        }

        private void btnOverrideSearch_Click(object sender, EventArgs e)
        {
            if (sender == null) return;
            googleIt(txtQuery.Text);
        }
        private void CellClick(object _sender, EventArgs _e)
        {
            var e      = (MouseEventArgs)_e;
            var sender = (clickHandler)_sender;
            var p      = (imageCell)sender.Parent;

            if (e.Button == MouseButtons.Left)
            {
                if (p.Text == @"embeded" || !string.IsNullOrEmpty(sender.storage))
                {
                    var viewer = p.Text == @"embeded"? new frmShowFull(EmbededArt) : new frmShowFull(sender.storage, chkCrop.Checked);
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
                foreach (var c in p.Parent.Controls) 
                    if (c is imageCell) 
                        if (c == p) continue; 
                        else ((imageCell)c).UnCheck();
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
                googleIt(currentJob[Fields.Artist] + " " + currentJob[Fields.Album]);
            }
        }

        private void unselectCells() { foreach (var c in this.Controls) if (c is imageCell) ((imageCell)c).UnCheck(); }

        private string getCurrentArtworkUrl(bool uncheckAll = true)
        {
            string result = "";
            foreach (var c in this.Controls)
                if (c is imageCell)
                {
                    var j = (imageCell)c;
                    if (!j.Checked) continue;
                    result = j.ClickHandler.storage;
                }

            if (uncheckAll) unselectCells();
            return result;
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
                cellEmbeded.Image = EmbededArt = (Image)ic.ConvertFrom(track.Tag.Pictures[0].Data.Data);
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

        private void saveArtWorkWorker(List<string> files, string artworkURL, int jobID)
        {
            var artwork = httpRequest.getPicture(artworkURL);
            if (artwork == null || artwork.Width == 0 || artwork.Height == 0)
            {
                jCon.Jobs.Tables[Fields.Tracks].Rows[jobID][Fields.Process] = false;
                return;
            }

            if (chkResize.Checked && (artwork.Width > numSize.Value || artwork.Height > numSize.Value))
                artwork = Tools.resizeImage(artwork, new Size((int)numSize.Value, (int)numSize.Value));

            if (chkCrop.Checked) artwork = Tools.CropImage(artwork);

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
                googleIt(currentJob[Fields.Artist] + " " + currentJob[Fields.Album]);
                chkSkip.Checked = false;
                btnPrev.Enabled = (int)currentJob[Fields.ID] > jCon.BottomMargin;
                btnNext.Enabled = (int)currentJob[Fields.ID] <= jCon.TopMargin;
            }
        }
    }
}
