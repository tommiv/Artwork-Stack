using System;
using System.IO;
using System.Net;
using System.Windows.Forms;
using System.Web.Script.Serialization;
using System.Threading;
using System.Drawing;

namespace Artwork_Stack
{
    public partial class frmShowResults : Form
    {
        public frmShowResults()
        {
            InitializeComponent();
        }

        private void frmShowResults_Load(object sender, EventArgs e)
        {

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
            for (int i = 0; i < p.i; i++)
            {
                WebRequest req = WebRequest.Create("https://ajax.googleapis.com/ajax/services/search/images?v=1.0&rsz=" + p.rsz + "&start=" + p.i * p.rsz + "&q=" + p.query);
                Stream resp = req.GetResponse().GetResponseStream();
                StreamReader reader = new StreamReader(resp);
                JavaScriptSerializer jsonParser = new JavaScriptSerializer();
                gImgAPI gImgAPIResult = jsonParser.Deserialize<gImgAPI>(reader.ReadToEnd());

                for (int j = 0; j < p.rsz; j++)
                {
                    getIMGWorkerParams parameters = new getIMGWorkerParams(gImgAPIResult.responseData.results[j].tbUrl, i, j, 100, 100);
                    Thread thread = new Thread(getIMGWorker);
                    thread.Start(parameters);
                }
            }
        }

        struct getIMGWorkerParams
        {
            public string url;
            public int ix; // thread position
            public int iy;
            public int w; // img thumb width
            public int h;
            public getIMGWorkerParams(string _url, int _ix, int _iy, int _w, int _h)
            { 
                url = _url; ix = _ix; iy = _iy; w = _w; h = _h;
            }
        }
        private void getIMGWorker(Object P)
        {
            getIMGWorkerParams p = (getIMGWorkerParams)P;
            Image thumb = httpRequest.getPicture(p.url);
            this.Invoke((Action)(() =>
            {
                PictureBox pb = new PictureBox();
                pb.Image = thumb;
                pb.Size = new System.Drawing.Size(100, 100);
                pb.Location = new Point(p.ix*p.w, p.iy*p.h);
                pb.SizeMode = PictureBoxSizeMode.Zoom;
                pb.BorderStyle = BorderStyle.FixedSingle;
                Controls.Add(pb);
            }
            ));
        }

        private void button1_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 4; i++)
            {
                gImgAPIWorkerParams parameters = new gImgAPIWorkerParams("Deftones - adrenaline", 4, i);
                Thread th = new Thread(gImgAPIWorker);
                th.Start(parameters);
            }
        }
    }
}
