namespace Artwork_Stack
{
    partial class frmShowFull
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.pictMain = new System.Windows.Forms.PictureBox();
            this.prgrssBar = new System.Windows.Forms.ProgressBar();
            ((System.ComponentModel.ISupportInitialize)(this.pictMain)).BeginInit();
            this.SuspendLayout();
            // 
            // pictMain
            // 
            this.pictMain.Location = new System.Drawing.Point(0, 0);
            this.pictMain.Name = "pictMain";
            this.pictMain.Size = new System.Drawing.Size(500, 500);
            this.pictMain.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictMain.TabIndex = 0;
            this.pictMain.TabStop = false;
            this.pictMain.Click += new System.EventHandler(this.pictMain_Click);
            // 
            // prgrssBar
            // 
            this.prgrssBar.Location = new System.Drawing.Point(88, 214);
            this.prgrssBar.MarqueeAnimationSpeed = 30;
            this.prgrssBar.Name = "prgrssBar";
            this.prgrssBar.Size = new System.Drawing.Size(306, 23);
            this.prgrssBar.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.prgrssBar.TabIndex = 1;
            this.prgrssBar.Value = 50;
            // 
            // frmShowFull
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(500, 500);
            this.Controls.Add(this.prgrssBar);
            this.Controls.Add(this.pictMain);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmShowFull";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "frmShowFull";
            this.Load += new System.EventHandler(this.frmShowFull_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictMain)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictMain;
        private System.Windows.Forms.ProgressBar prgrssBar;
    }
}