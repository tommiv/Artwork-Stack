namespace Artwork_Stack.Controls
{
    partial class imageCell
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

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.Picture = new System.Windows.Forms.PictureBox();
            this.caption = new System.Windows.Forms.Label();
            this.ClickHandler = new clickHandler();
            ((System.ComponentModel.ISupportInitialize)(this.Picture)).BeginInit();
            this.SuspendLayout();
            // 
            // Picture
            // 
            this.Picture.Location = new System.Drawing.Point(0, 0);
            this.Picture.Name = "Picture";
            this.Picture.Size = new System.Drawing.Size(150, 150);
            this.Picture.TabIndex = 0;
            this.Picture.TabStop = false;
            // 
            // caption
            // 
            this.caption.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.caption.Location = new System.Drawing.Point(0, 154);
            this.caption.Name = "caption";
            this.caption.Size = new System.Drawing.Size(150, 26);
            this.caption.TabIndex = 1;
            // 
            // lbl
            // 
            this.ClickHandler.Location = new System.Drawing.Point(0, 0);
            this.ClickHandler.Name = "lbl";
            this.ClickHandler.Size = new System.Drawing.Size(100, 23);
            this.ClickHandler.TabIndex = 3;
            // 
            // imageCell
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.caption);
            this.Controls.Add(this.Picture);
            this.Controls.Add(this.ClickHandler);
            this.Name = "imageCell";
            this.Size = new System.Drawing.Size(150, 180);
            ((System.ComponentModel.ISupportInitialize)(this.Picture)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox Picture;
        private System.Windows.Forms.Label caption;
        public clickHandler ClickHandler;
    }
}
