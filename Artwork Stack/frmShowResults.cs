using System;
using System.IO;
using System.Net;
using System.Windows.Forms;
using System.Web.Script.Serialization;
using System.Threading;
using System.Drawing;
using ImageCell;

namespace Artwork_Stack
{
    public partial class frmShowResults : Form
    {
        public frmShowResults() { InitializeComponent(); }

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
                    cell[ii, jj].Click += new EventHandler(CellClick); 
                    Controls.Add(cell[ii, jj]);
                }
            button1_Click(null, null); // HACK: for "initialize" threads // TODO: google it
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
            gImgAPIWorkerParams p = (gImgAPIWorkerParams)P;
            WebRequest req = WebRequest.Create("https://ajax.googleapis.com/ajax/services/search/images?v=1.0&rsz=" + p.rsz + "&start=" + p.i * p.rsz + "&q=" + p.query);
            Stream resp = req.GetResponse().GetResponseStream();
            StreamReader reader = new StreamReader(resp);
            JavaScriptSerializer jsonParser = new JavaScriptSerializer();
            gImgAPI gImgAPIResult = jsonParser.Deserialize<gImgAPI>(reader.ReadToEnd());

            if (gImgAPIResult.responseData.results.Length == 0) return;

            Thread[] thread = new Thread[p.rsz];
            for (int j = 0; j < p.rsz; j++)
            {
                string caption = string.Format("{0}x{1}: {2}\n{3}", 
                                                gImgAPIResult.responseData.results[j].width, 
                                                gImgAPIResult.responseData.results[j].height,
                                                gImgAPIResult.responseData.results[j].url.Substring(gImgAPIResult.responseData.results[j].url.LastIndexOf('.')+1, 3),
                                                gImgAPIResult.responseData.results[j].visibleUrl
                );
                getIMGWorkerParams parameters = new getIMGWorkerParams(gImgAPIResult.responseData.results[j].tbUrl, gImgAPIResult.responseData.results[j].url, p.i, j, 100, 100, caption);
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
            getIMGWorkerParams p = (getIMGWorkerParams)P;
            Image thumb = httpRequest.getPicture(p.tburl);
            this.Invoke((Action)(() =>
            {
                cell[p.ix, p.iy].Image                = thumb;
                cell[p.ix, p.iy].Caption              = p.caption;
                cell[p.ix, p.iy].ClickHandler.storage = p.url;
            }
            ));
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Thread[] thread = new Thread[4];
            for (int i = 0; i < 4; i++)
            {
                gImgAPIWorkerParams parameters = new gImgAPIWorkerParams(txtQuery.Text, 4, i);
                thread[i] = new Thread(gImgAPIWorker);
                thread[i].Start(parameters);
            }
        }

        private void CellClick(object sender, EventArgs e) // string url
        {
            string url = "";
            try { url = ((clickHandler)sender).storage; } catch { }; // HACK: it's very simple
            if (url != "")
            {
                frmShowFull frm = new frmShowFull(url);
                frm.ShowDialog();
            }
        }
    }
}
