using System;
using System.Drawing;
using System.Windows.Forms;

namespace ImageCell
{
    public partial class imageCell : UserControl
    {
        public imageCell(int w, int h, int x, int y)
        {
            InitializeComponent();
            this.Size = new Size(w, h);
            this.Location = new Point(x, y);

            this.Picture.Size = new Size(w, h - 30);
            this.Picture.SizeMode = PictureBoxSizeMode.Zoom;
            this.Picture.BorderStyle = BorderStyle.FixedSingle;
            this.Picture.Location = new Point(0, 0);

            this.caption.Size = new Size(w, 26);
            this.caption.Location = new Point(0, h - 26);

            this.ClickHandler.Size = new Size(w, h);
            this.ClickHandler.Location = new Point(0, 0);
            this.ClickHandler.BringToFront();
        }

        public Image Image { set { this.Picture.Image = value; } }
        public string Caption { set { this.caption.Text = value; } }

        public new event EventHandler Click
        {
            add
            {
                base.Click += value;
                foreach (Control c in Controls) c.Click += value;
            }
            remove
            {
                base.Click -= value;
                foreach (Control c in Controls) c.Click -= value;
            }
        }
    }
}
