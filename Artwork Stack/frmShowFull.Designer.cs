﻿namespace Artwork_Stack
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
            this.picBusy = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lblPictureRes = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictMain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBusy)).BeginInit();
            this.SuspendLayout();
            // 
            // pictMain
            // 
            this.pictMain.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.pictMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictMain.ErrorImage = global::Artwork_Stack.Properties.Resources.noartwork;
            this.pictMain.Location = new System.Drawing.Point(0, 0);
            this.pictMain.Margin = new System.Windows.Forms.Padding(0);
            this.pictMain.Name = "pictMain";
            this.pictMain.Size = new System.Drawing.Size(500, 500);
            this.pictMain.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictMain.TabIndex = 0;
            this.pictMain.TabStop = false;
            this.pictMain.Click += new System.EventHandler(this.pictMain_Click);
            // 
            // picBusy
            // 
            this.picBusy.Image = global::Artwork_Stack.Properties.Resources.ajax_loader;
            this.picBusy.Location = new System.Drawing.Point(201, 184);
            this.picBusy.Name = "picBusy";
            this.picBusy.Size = new System.Drawing.Size(100, 100);
            this.picBusy.TabIndex = 1;
            this.picBusy.TabStop = false;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Salmon;
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.Location = new System.Drawing.Point(0, 521);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(250, 22);
            this.label1.TabIndex = 2;
            this.label1.Text = "LMB to discard and close";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.LightGreen;
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label2.Location = new System.Drawing.Point(250, 521);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(250, 22);
            this.label2.TabIndex = 3;
            this.label2.Text = "RMB to select this and close";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblPictureRes
            // 
            this.lblPictureRes.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblPictureRes.Location = new System.Drawing.Point(0, 500);
            this.lblPictureRes.Name = "lblPictureRes";
            this.lblPictureRes.Size = new System.Drawing.Size(500, 21);
            this.lblPictureRes.TabIndex = 4;
            this.lblPictureRes.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // frmShowFull
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(500, 543);
            this.Controls.Add(this.lblPictureRes);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.picBusy);
            this.Controls.Add(this.pictMain);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmShowFull";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "frmShowFull";
            ((System.ComponentModel.ISupportInitialize)(this.pictMain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBusy)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictMain;
        private System.Windows.Forms.PictureBox picBusy;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblPictureRes;
    }
}