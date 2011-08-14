﻿using System;
using System.Data;
using System.IO;
using System.Net;
using System.Windows.Forms;
using System.Web.Script.Serialization;
using System.Threading;
using System.Drawing;
using ImageCell;
using TagLib;

//TODO: figure out some artworks not showed in explorer; add grouping by album
//TODO: change colors, properly size joblist;
//TODO: create showFull constructor with Image for embeded art && store this image in formDoWork property
namespace Artwork_Stack
{
    public partial class formDoWork : Form
    {
        private readonly JobController jCon = new JobController();
        private DataRow currentJob;
        public formDoWork(string Path, bool Recurse)
        {
            InitializeComponent();
            jCon.TraverseFolder(Path, Recurse);
            currentJob = jCon.Jobs.Tables["Tracks"].Rows[0];
        }

        struct grid
        {
            public const int i = 4;
            public const int j = 4;
            public const int w = 100;
            public const int h = 130;
            public const int pw = 4; // Space between cells
            public const int ph = 4;
            public const int lm = 12; // Left margin
            public const int tm = 12;
        }

        readonly imageCell[,] cell        = new imageCell[grid.i, grid.j];
        private  imageCell cellEmbeded;

        private void formDoWork_Shown(object sender, EventArgs e)
        {
            picBusy.Location = new Point(0,0);
            picBusy.Size     = this.Size;

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

        private struct gImgAPIWorkerParams
        {
            public string query;
            public int rsz; // results per page
            public int i;   // thread position
            public gImgAPIWorkerParams(string _query, int _rsz, int _i)
            {
                query = _query; rsz = _rsz; i = _i;
            }
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
        struct getIMGWorkerParams
        {
            public string tburl;
            public string url;
            public int ix; // thread position
            public int iy;
            public int w;
            public int h;
            public string caption;
            public getIMGWorkerParams(string _tburl, string _url, int _ix, int _iy, int _w, int _h, string _caption)
            {
                tburl = _tburl; url = _url; ix = _ix; iy = _iy; w = _w; h = _h; caption = _caption;
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
                //Thread.Sleep(500);
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

            if (e.Button == MouseButtons.Left && p.Text != @"embeded")
            {
                string url = "";
                try { url = sender.storage; } catch { }; // HACK: it's very simple
                if (url != "")
                {
                    var viewer = new frmShowFull(url);
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

        private void btnJobs_Click(object sender, EventArgs e) { jCon.ShowJobList(); }

        private void buttonCycleClick(object sender, EventArgs e)
        {
            DataRow stored = currentJob;
            int curJobIndex = (int)currentJob["ID"];

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
                if (jCon.PendingJobsCount == 0) break;
                DataRow newdr = jCon.Jobs.Tables["Tracks"].Rows[i];
                if ((bool)newdr["done"])
                {
                    if ( isForwardClick && i != endIndex) { i++; continue; }
                    if (!isForwardClick && i != endIndex) { i--; continue; }
                }
                else
                {
                    currentJob = newdr; break;
                }
            }

            if (!chkSkip.Checked) saveArtWork(stored, getCurrentArtworkUrl());
            else
            {
                jCon.SetJobIsDone(curJobIndex);
                unselectCells();
            }

            if (jCon.PendingJobsCount == 0)
            {
                btnPrev.Enabled = btnNext.Enabled = false;
                MessageBox.Show(@"That's all, folks!");
            }
            else
            {
                btnPrev.Enabled = (int) currentJob["ID"] >  jCon.BottomMargin;
                btnNext.Enabled = (int) currentJob["ID"] <= jCon.TopMargin;
                showTrackInfo();
                googleIt(currentJob["Artist"] + " " + currentJob["Album"]);
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
            txtQuery.Text = jCon.CreateQueryString(int.Parse(currentJob["ID"].ToString()));
            gridCurrentJob.Rows.Clear();
            gridCurrentJob.Rows.Add("Job", int.Parse(currentJob["ID"].ToString()) + "/" + (jCon.JobsCount - 1));
            gridCurrentJob.Rows.Add("Path", currentJob["Path"].ToString());
            gridCurrentJob.Rows.Add("Artist", currentJob["Artist"].ToString());
            gridCurrentJob.Rows.Add("Track", currentJob["Title"].ToString());
            gridCurrentJob.Rows.Add("Album", currentJob["Album"].ToString());
            var track = TagLib.File.Create(currentJob["Path"].ToString());
            gridCurrentJob.Rows.Add("Art in file", track.Tag.Pictures.GetLength(0));
            if (track.Tag.Pictures.GetLength(0) > 0)
            {
                var ic = new ImageConverter();
                cellEmbeded.Image = (Image)ic.ConvertFrom(track.Tag.Pictures[0].Data.Data);
                cellEmbeded.Caption = @"Embedded Art";
            }
            else
            {
                cellEmbeded.Image = Properties.Resources.noartwork;
                cellEmbeded.Caption = @"No Embedded Art";
            }
            track.Dispose();
        }

        private void saveArtWork(DataRow job, string artworkUrl)
        {
            if (String.IsNullOrEmpty(artworkUrl)) return;
            (new Thread(() => saveArtWorkWorker(job["Path"].ToString(), artworkUrl, (int)job["ID"]))).Start();
        }

        private void saveArtWorkWorker(string file, string artworkURL, int jobID)
        {
            Byte[] artwork = httpRequest.getStream(artworkURL);
            if (artwork == null) return;
            var artworkInMem = new MemoryStream(artwork);
            var pic = new Picture(ByteVector.FromStream(artworkInMem));
            Picture[] artworkFrame = { pic };
            var track = TagLib.File.Create(file);
            track.Tag.Pictures = artworkFrame;
            track.Save();
            jCon.SetJobIsDone(jobID);
        }
    }
}
