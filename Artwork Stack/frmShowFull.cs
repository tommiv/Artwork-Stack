using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Artwork_Stack
{
    public partial class frmShowFull : Form
    {
        string url;
        Image mainImage;
        public frmShowFull(string _url)
        {
            InitializeComponent();
            var bg = new BackgroundWorker();
            bg.DoWork += this.getImgWorker;
            bg.RunWorkerCompleted +=bg_RunWorkerCompleted;
            bg.RunWorkerAsync();
            url = _url;
        }

        private void frmShowFull_Load(object sender, EventArgs e)
        {

        }

        private void pictMain_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void getImgWorker(object sender, DoWorkEventArgs e)
        {
            mainImage = httpRequest.getPicture(url);
        }

        private void bg_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            prgrssBar.Visible = false;
            pictMain.Image = httpRequest.getPicture(url);
        }
          

    }
}
