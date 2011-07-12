﻿namespace Artwork_Stack
{
    partial class formDoWork
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.btnSearch = new System.Windows.Forms.Button();
            this.txtQuery = new System.Windows.Forms.TextBox();
            this.btnJobs = new System.Windows.Forms.Button();
            this.btnNext = new System.Windows.Forms.Button();
            this.btnPrev = new System.Windows.Forms.Button();
            this.gridCurrentJob = new System.Windows.Forms.DataGridView();
            this.Parameter = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Value = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.picEmbeddedArt = new System.Windows.Forms.PictureBox();
            this.lblEmbedded = new System.Windows.Forms.Label();
            this.txtDebug = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.gridCurrentJob)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picEmbeddedArt)).BeginInit();
            this.SuspendLayout();
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(725, 38);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(99, 23);
            this.btnSearch.TabIndex = 0;
            this.btnSearch.Text = "Override Search";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnOverrideSearch_Click);
            // 
            // txtQuery
            // 
            this.txtQuery.Location = new System.Drawing.Point(502, 12);
            this.txtQuery.Name = "txtQuery";
            this.txtQuery.Size = new System.Drawing.Size(322, 20);
            this.txtQuery.TabIndex = 3;
            // 
            // btnJobs
            // 
            this.btnJobs.Location = new System.Drawing.Point(627, 491);
            this.btnJobs.Name = "btnJobs";
            this.btnJobs.Size = new System.Drawing.Size(75, 23);
            this.btnJobs.TabIndex = 4;
            this.btnJobs.Text = "Jobs List";
            this.btnJobs.UseVisualStyleBackColor = true;
            this.btnJobs.Click += new System.EventHandler(this.btnJobs_Click);
            // 
            // btnNext
            // 
            this.btnNext.Location = new System.Drawing.Point(749, 491);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(75, 23);
            this.btnNext.TabIndex = 5;
            this.btnNext.Text = "Next >>";
            this.btnNext.UseVisualStyleBackColor = true;
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // btnPrev
            // 
            this.btnPrev.Location = new System.Drawing.Point(502, 491);
            this.btnPrev.Name = "btnPrev";
            this.btnPrev.Size = new System.Drawing.Size(75, 23);
            this.btnPrev.TabIndex = 6;
            this.btnPrev.Text = "<< Prev";
            this.btnPrev.UseVisualStyleBackColor = true;
            this.btnPrev.Click += new System.EventHandler(this.btnPrev_Click);
            // 
            // gridCurrentJob
            // 
            this.gridCurrentJob.AllowUserToAddRows = false;
            this.gridCurrentJob.AllowUserToDeleteRows = false;
            this.gridCurrentJob.AllowUserToResizeRows = false;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.LightCyan;
            this.gridCurrentJob.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle2;
            this.gridCurrentJob.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.gridCurrentJob.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridCurrentJob.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.gridCurrentJob.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.gridCurrentJob.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridCurrentJob.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Parameter,
            this.Value});
            this.gridCurrentJob.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.gridCurrentJob.Location = new System.Drawing.Point(502, 82);
            this.gridCurrentJob.MultiSelect = false;
            this.gridCurrentJob.Name = "gridCurrentJob";
            this.gridCurrentJob.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.gridCurrentJob.RowHeadersVisible = false;
            this.gridCurrentJob.RowTemplate.Height = 20;
            this.gridCurrentJob.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.gridCurrentJob.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridCurrentJob.ShowCellErrors = false;
            this.gridCurrentJob.ShowEditingIcon = false;
            this.gridCurrentJob.ShowRowErrors = false;
            this.gridCurrentJob.Size = new System.Drawing.Size(322, 195);
            this.gridCurrentJob.TabIndex = 7;
            // 
            // Parameter
            // 
            this.Parameter.HeaderText = "Parameter";
            this.Parameter.Name = "Parameter";
            this.Parameter.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Parameter.Width = 61;
            // 
            // Value
            // 
            this.Value.HeaderText = "Value";
            this.Value.Name = "Value";
            this.Value.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Value.Width = 40;
            // 
            // picEmbeddedArt
            // 
            this.picEmbeddedArt.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picEmbeddedArt.ErrorImage = global::Artwork_Stack.Properties.Resources.noartwork;
            this.picEmbeddedArt.Image = global::Artwork_Stack.Properties.Resources.noartwork;
            this.picEmbeddedArt.InitialImage = global::Artwork_Stack.Properties.Resources.noartwork;
            this.picEmbeddedArt.Location = new System.Drawing.Point(709, 152);
            this.picEmbeddedArt.Name = "picEmbeddedArt";
            this.picEmbeddedArt.Size = new System.Drawing.Size(100, 100);
            this.picEmbeddedArt.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picEmbeddedArt.TabIndex = 8;
            this.picEmbeddedArt.TabStop = false;
            // 
            // lblEmbedded
            // 
            this.lblEmbedded.Location = new System.Drawing.Point(709, 255);
            this.lblEmbedded.Name = "lblEmbedded";
            this.lblEmbedded.Size = new System.Drawing.Size(100, 19);
            this.lblEmbedded.TabIndex = 9;
            this.lblEmbedded.Text = "Embedded Art";
            this.lblEmbedded.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // txtDebug
            // 
            this.txtDebug.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtDebug.Location = new System.Drawing.Point(502, 295);
            this.txtDebug.Multiline = true;
            this.txtDebug.Name = "txtDebug";
            this.txtDebug.Size = new System.Drawing.Size(322, 178);
            this.txtDebug.TabIndex = 10;
            // 
            // formDoWork
            // 
            this.AcceptButton = this.btnSearch;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(836, 553);
            this.Controls.Add(this.txtDebug);
            this.Controls.Add(this.lblEmbedded);
            this.Controls.Add(this.picEmbeddedArt);
            this.Controls.Add(this.gridCurrentJob);
            this.Controls.Add(this.btnPrev);
            this.Controls.Add(this.btnNext);
            this.Controls.Add(this.btnJobs);
            this.Controls.Add(this.txtQuery);
            this.Controls.Add(this.btnSearch);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "formDoWork";
            this.Load += new System.EventHandler(this.frmShowResults_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gridCurrentJob)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picEmbeddedArt)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.TextBox txtQuery;
        private System.Windows.Forms.Button btnJobs;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.Button btnPrev;
        private System.Windows.Forms.DataGridView gridCurrentJob;
        private System.Windows.Forms.DataGridViewTextBoxColumn Parameter;
        private System.Windows.Forms.DataGridViewTextBoxColumn Value;
        private System.Windows.Forms.PictureBox picEmbeddedArt;
        private System.Windows.Forms.Label lblEmbedded;
        private System.Windows.Forms.TextBox txtDebug;


    }
}

