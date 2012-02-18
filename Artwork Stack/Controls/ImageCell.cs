using System;
using System.Drawing;
using System.Windows.Forms;

namespace Artwork_Stack.Controls
{
    public partial class imageCell : UserControl
    {
        private const int margin = 10;

        private imageCell()
        {
            
        }

        public imageCell(int w, int h, int x, int y)
        {
            InitializeComponent();
            this.Size = new Size(w, h);
            this.BorderStyle = BorderStyle.None;
            this.Location = new Point(x, y);

            this.Picture.Size = new Size(w - margin*2, h - 30 - margin*2);
            this.Picture.SizeMode = PictureBoxSizeMode.Zoom;
            this.Picture.BorderStyle = BorderStyle.FixedSingle;
            this.Picture.Location = new Point(margin, margin);

            this.caption.Size = new Size(w - margin*2, 26);
            this.caption.Location = new Point(margin, h - 26 - margin);

            this.ClickHandler.Size = new Size(w, h);
            this.ClickHandler.Location = new Point(0, 0);
            this.ClickHandler.BringToFront();
        }

        public Image Thumbnail
        {
            set
            {
                if (this.InvokeRequired)
                {
                    this.Invoke((Action) (() => this.Picture.Image = value));
                }
                else
                {
                    this.Picture.Image = value;
                }
            }
        }

        public Image  FullSize       { get; set; }
        public string FullSizeUrl    { get; set; }
        public object AdditionalInfo { get; set; }
        public bool   IsEmbeded      { get; set; }

        public string Caption { set { this.caption.Text = value; } }
        public void Check()   { this.BackColor = Color.FromArgb(255, 160, 255, 160); _checked = true; }
        public void UnCheck() { this.BackColor = SystemColors.Control; _checked = false; }
        public bool Checked   { get { return _checked; } }
        private bool _checked;
        //public bool Initated;

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
