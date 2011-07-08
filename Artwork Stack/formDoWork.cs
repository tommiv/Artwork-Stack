using System;
using System.Data;
using System.IO;
using System.Net;
using System.Windows.Forms;
using System.Web.Script.Serialization;
using System.Threading;
using System.Drawing;
using ImageCell;

namespace Artwork_Stack
{
    public partial class formDoWork : Form
    {
        private JobController jCon = new JobController();
        private DataRow currentJob;
        public formDoWork(string Path)
        {
            InitializeComponent();
            jCon.TraverseFolder(Path);
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

        imageCell[,] cell = new imageCell[grid.i, grid.j];

        private void frmShowResults_Load(object sender, EventArgs e)
        {
            for (int ii = 0; ii < grid.i; ii++)
                for (int jj = 0; jj < grid.j; jj++)
                {
                    cell[ii, jj] = new imageCell(grid.w, grid.h, grid.lm + ii * grid.w + ii * grid.pw, grid.tm + jj * grid.h + jj * grid.ph);
                    cell[ii, jj].Click += CellClick; 
                    Controls.Add(cell[ii, jj]);
                }
            picEmbeddedArt.Image = Properties.Resources.noartwork;
            setNewJob(currentJob);
            txtQuery.Text = jCon.CreateQueryString(int.Parse(currentJob["ID"].ToString()));
            //googleIt(txtQuery.Text);
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
            var gImgAPIResult = jsonParser.Deserialize<gImgAPI>(reader.ReadToEnd()); // TODO: use Dynamic?

            if (gImgAPIResult.responseData.results.Length == 0) return;

            var thread = new Thread[p.rsz];
            for (int j = 0; j < p.rsz; j++)
            {
                string caption = string.Format("{0}x{1}: {2}\n{3}", 
                                                gImgAPIResult.responseData.results[j].width, 
                                                gImgAPIResult.responseData.results[j].height,
                                                gImgAPIResult.responseData.results[j].url.Substring(gImgAPIResult.responseData.results[j].url.LastIndexOf('.')+1, 3),
                                                gImgAPIResult.responseData.results[j].visibleUrl
                );
                var parameters = new getIMGWorkerParams(gImgAPIResult.responseData.results[j].tbUrl, gImgAPIResult.responseData.results[j].url, p.i, j, 100, 100, caption);
                thread[j] = new Thread(getIMGWorker);
                thread[j].Start(parameters);
            }
        }
        struct getIMGWorkerParams // TODO: remove useless params
        {
            public string tburl;
            public string url;
            public int ix; // thread position
            public int iy;
            public int w;  // img thumb width
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
        private void googleIt(string query)
        {
            var thread = new Thread[4];
            for (int i = 0; i < 4; i++)
            {
                var parameters = new gImgAPIWorkerParams(query, 4, i);
                thread[i] = new Thread(gImgAPIWorker);
                thread[i].Start(parameters);
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (sender == null) return;
            googleIt(txtQuery.Text);
        }
        private static void CellClick(object sender, EventArgs e) // string url
        {
            string url = "";
            try { url = ((clickHandler)sender).storage; } catch { }; // HACK: it's very simple
            if (url != "") new frmShowFull(url).ShowDialog();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            jCon.ShowJobList();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            int curJobIndex = int.Parse(currentJob["ID"].ToString());
            if (curJobIndex != jCon.JobsCount)
            {
                for (int i = curJobIndex + 1; i < jCon.JobsCount; i++)
                {
                    DataRow newdr = jCon.Jobs.Tables["Tracks"].Rows[i];
                    if (!bool.Parse(newdr["done"].ToString()))
                    {
                        setNewJob(newdr); return;
                    }
                }
            }
            for (int i = 0; i < curJobIndex; i++)
            {
                DataRow newdr = jCon.Jobs.Tables["Tracks"].Rows[i];
                if (!bool.Parse(newdr["done"].ToString()))
                {
                    setNewJob(newdr); return;
                }
            }
            MessageBox.Show(@"That's all, folks!");
        }
        private void btnPrev_Click(object sender, EventArgs e)
        {
            int curJobIndex = int.Parse(currentJob["ID"].ToString());
            if (curJobIndex != 0)
            {
                for (int i = curJobIndex - 1; i < jCon.JobsCount; i--)
                {
                    DataRow newdr = jCon.Jobs.Tables["Tracks"].Rows[i];
                    if (!bool.Parse(newdr["done"].ToString()))
                    {
                        setNewJob(newdr); return;
                    }
                }
            }
            for (int i = jCon.JobsCount - 1; i > curJobIndex; i--)
            {
                DataRow newdr = jCon.Jobs.Tables["Tracks"].Rows[i];
                if (!bool.Parse(newdr["done"].ToString()))
                {
                    setNewJob(newdr); return;
                }
            }
            MessageBox.Show(@"That's all, folks!");
        }
        private void setNewJob(DataRow newdr)
        {
            // Do current job HERE

            currentJob = newdr;
            txtQuery.Text = jCon.CreateQueryString(int.Parse(currentJob["ID"].ToString()));
            //googleIt(txtQuery.Text);

            gridCurrentJob.Rows.Clear();
            gridCurrentJob.Rows.Add("Job",    int.Parse(currentJob["ID"].ToString()) + "/" + (jCon.JobsCount-1));
            gridCurrentJob.Rows.Add("Path",   currentJob["Path"].ToString());
            gridCurrentJob.Rows.Add("Artist", currentJob["Artist"].ToString());
            gridCurrentJob.Rows.Add("Track",  currentJob["Title"].ToString());
            gridCurrentJob.Rows.Add("Album",  currentJob["Album"].ToString());
            var track = TagLib.File.Create(currentJob["Path"].ToString());
            gridCurrentJob.Rows.Add("Art in file", track.Tag.Pictures.GetLength(0));
            if (track.Tag.Pictures.GetLength(0) > 0)
            {
                var ic = new ImageConverter();
                picEmbeddedArt.Image = (Image)ic.ConvertFrom(track.Tag.Pictures[0].Data.Data);
                lblEmbedded.Text = @"Embedded Art";
            }
            else
            {
                picEmbeddedArt.Image = Properties.Resources.noartwork;
                lblEmbedded.Text = @"No Embedded Art";
            }
        }
    }
}
