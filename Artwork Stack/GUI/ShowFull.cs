using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Artwork_Stack.Tools;

namespace Artwork_Stack.GUI
{
    public partial class ShowFull : Form
    {
        readonly string url;
        Image mainImage;
        public bool Selected;
        public bool NotAvailable;

        private readonly bool crop;

        public ShowFull(string _url, bool _crop = false)
        {
            InitializeComponent();
            url = _url;
            crop = _crop;
            var bg = new BackgroundWorker();
            bg.DoWork += getImgWorker;
            bg.RunWorkerCompleted +=bg_RunWorkerCompleted;
            bg.RunWorkerAsync();
        }
        public ShowFull(Image img, bool _crop = false)
        {
            InitializeComponent();
            crop = _crop;
            mainImage = img;
            bg_RunWorkerCompleted(null, null);
        }

        private void getImgWorker(object sender, DoWorkEventArgs e)
        {
            mainImage = Http.getPicture(url);
        }
        
        private void Placeholder_Paint(object sender, PaintEventArgs e)
        {
            ProcessPaint(e.Graphics);
            Placeholder.Visible = true;
        }

        private void bg_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            picBusy.Visible = false;
            if (mainImage == null)
            {
                MessageBox.Show(Verbal.PicNotAvailable);
                NotAvailable = true;
                this.Close();
                return;
            }
            
            bool resize = mainImage.Width > Placeholder.Width || mainImage.Height > Placeholder.Height;
            
            lblPictureRes.Text = string.Format(
                "Resolution: {0}x{1}. Resized: {2}",
                mainImage.Width,
                mainImage.Height,
                resize? "yes" : "no"
            );

            if (resize) mainImage = Images.ResizeImage(mainImage, Placeholder.Size);

            ProcessPaint();
            Placeholder.Visible = true;
        }

        private void ProcessClick(object sender, EventArgs e)
        {
            if (((MouseEventArgs)e).Button == MouseButtons.Right) Selected = true;
            this.Close();
        }

        private void ProcessPaint(Graphics gfx = null)
        {
            bool needDispose = gfx == null;
            if (gfx == null) gfx = Placeholder.CreateGraphics();

            // draw background
            var backRect = new Rectangle(Placeholder.Location.X, Placeholder.Location.Y, Placeholder.Width, Placeholder.Height);
            gfx.FillRectangle(SystemBrushes.Control, backRect);

            // draw main image
            var imgRect = new Rectangle();
            if (mainImage.Width > Placeholder.Width || mainImage.Height > Placeholder.Height)
            {
                imgRect.X = imgRect.X = 0;
                imgRect.Width = imgRect.Height = Placeholder.Width;
            }
            else
            {
                imgRect.X = (Placeholder.Width  - mainImage.Width) / 2;
                imgRect.Y = (Placeholder.Height - mainImage.Height) / 2;
                imgRect.Width  = mainImage.Width;
                imgRect.Height = mainImage.Height;
            }
            gfx.DrawImage(mainImage, imgRect);

            // draw crop rectangles
            if (!crop || Math.Abs(mainImage.Width - mainImage.Height) <= 1) return;

            using (var brush = new SolidBrush(Color.FromArgb(128, 0, 0, 0)))
            {
                var r = new Rectangle[2];
                int unused = Math.Abs(mainImage.Width - mainImage.Height);
                if (mainImage.Width > mainImage.Height)
                {
                    r[0] = new Rectangle(imgRect.Location.X,                                   imgRect.Location.Y, unused / 2, mainImage.Height);
                    r[1] = new Rectangle(imgRect.Location.X + imgRect.Size.Width - unused / 2, imgRect.Location.Y, unused / 2, mainImage.Height);
                }
                else
                {
                    r[0] = new Rectangle(imgRect.Location.X, imgRect.Location.Y,                                    mainImage.Width, unused / 2);
                    r[1] = new Rectangle(imgRect.Location.X, imgRect.Location.Y + imgRect.Size.Height - unused / 2, mainImage.Width, unused / 2);
                }

                gfx.FillRectangles(brush, r);
            }

            if(needDispose) gfx.Dispose();
        }
    }
}
