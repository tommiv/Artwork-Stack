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
        public bool Selected     = false;
        public bool NotAvailable = false;
        public frmShowFull(string _url)
        {
            InitializeComponent();
            var bg = new BackgroundWorker();
            bg.DoWork += getImgWorker;
            bg.RunWorkerCompleted +=bg_RunWorkerCompleted;
            bg.RunWorkerAsync();
            url = _url;
        }
        public frmShowFull(Image img)
        {
            InitializeComponent();
            mainImage = img;
            bg_RunWorkerCompleted(null, null);
        }

        private void pictMain_Click(object sender, EventArgs e)
        {
            if (((MouseEventArgs)e).Button == MouseButtons.Right) Selected = true;
            this.Close();
        }

        private void getImgWorker(object sender, DoWorkEventArgs e) { mainImage = httpRequest.getPicture(url); }

        private void bg_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            picBusy.Visible = false;
            if (mainImage == null)
            {
                MessageBox.Show(@"Picture is not available");
                NotAvailable = true;
                this.Close();
                return;
            }
            pictMain.Image = mainImage;
            if (mainImage.Width > pictMain.Size.Width || mainImage.Height > pictMain.Size.Height)
                pictMain.SizeMode = PictureBoxSizeMode.Zoom;
            lblPictureRes.Text = string.Format(
                "Resolution: {0}x{1}. Resized: {2}",
                mainImage.Width,
                mainImage.Height,
                pictMain.SizeMode == PictureBoxSizeMode.Zoom? "yes" : "no"
            );
        }
          

    }
}
