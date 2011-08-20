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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.btnSearch = new System.Windows.Forms.Button();
            this.txtQuery = new System.Windows.Forms.TextBox();
            this.btnJobs = new System.Windows.Forms.Button();
            this.btnNext = new System.Windows.Forms.Button();
            this.btnPrev = new System.Windows.Forms.Button();
            this.gridCurrentJob = new System.Windows.Forms.DataGridView();
            this.chkSkip = new System.Windows.Forms.CheckBox();
            this.Parameter = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Value = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.picBusy = new Artwork_Stack.TransparentPictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.gridCurrentJob)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBusy)).BeginInit();
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
            this.btnNext.Click += new System.EventHandler(this.buttonCycleClick);
            // 
            // btnPrev
            // 
            this.btnPrev.Enabled = false;
            this.btnPrev.Location = new System.Drawing.Point(502, 491);
            this.btnPrev.Name = "btnPrev";
            this.btnPrev.Size = new System.Drawing.Size(75, 23);
            this.btnPrev.TabIndex = 6;
            this.btnPrev.Text = "<< Prev";
            this.btnPrev.UseVisualStyleBackColor = true;
            this.btnPrev.Click += new System.EventHandler(this.buttonCycleClick);
            // 
            // gridCurrentJob
            // 
            this.gridCurrentJob.AllowUserToAddRows = false;
            this.gridCurrentJob.AllowUserToDeleteRows = false;
            this.gridCurrentJob.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.White;
            this.gridCurrentJob.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.gridCurrentJob.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.gridCurrentJob.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridCurrentJob.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.gridCurrentJob.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.gridCurrentJob.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridCurrentJob.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Parameter,
            this.Value});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.gridCurrentJob.DefaultCellStyle = dataGridViewCellStyle2;
            this.gridCurrentJob.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.gridCurrentJob.GridColor = System.Drawing.SystemColors.ButtonHighlight;
            this.gridCurrentJob.Location = new System.Drawing.Point(502, 82);
            this.gridCurrentJob.MultiSelect = false;
            this.gridCurrentJob.Name = "gridCurrentJob";
            this.gridCurrentJob.ReadOnly = true;
            this.gridCurrentJob.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.gridCurrentJob.RowHeadersVisible = false;
            this.gridCurrentJob.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.gridCurrentJob.RowTemplate.Height = 20;
            this.gridCurrentJob.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.gridCurrentJob.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridCurrentJob.ShowCellErrors = false;
            this.gridCurrentJob.ShowEditingIcon = false;
            this.gridCurrentJob.ShowRowErrors = false;
            this.gridCurrentJob.Size = new System.Drawing.Size(322, 185);
            this.gridCurrentJob.TabIndex = 7;
            // 
            // chkSkip
            // 
            this.chkSkip.Appearance = System.Windows.Forms.Appearance.Button;
            this.chkSkip.Location = new System.Drawing.Point(749, 451);
            this.chkSkip.Name = "chkSkip";
            this.chkSkip.Size = new System.Drawing.Size(75, 24);
            this.chkSkip.TabIndex = 11;
            this.chkSkip.Text = "Skip";
            this.chkSkip.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.chkSkip.UseVisualStyleBackColor = true;
            // 
            // Parameter
            // 
            this.Parameter.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Parameter.HeaderText = "Parameter";
            this.Parameter.Name = "Parameter";
            this.Parameter.ReadOnly = true;
            this.Parameter.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Parameter.Width = 80;
            // 
            // Value
            // 
            this.Value.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Value.HeaderText = "Value";
            this.Value.Name = "Value";
            this.Value.ReadOnly = true;
            this.Value.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Value.Width = 242;
            // 
            // picBusy
            // 
            this.picBusy.BackColor = System.Drawing.Color.Transparent;
            this.picBusy.Image = global::Artwork_Stack.Properties.Resources.ajax_loader;
            this.picBusy.Location = new System.Drawing.Point(357, 205);
            this.picBusy.Name = "picBusy";
            this.picBusy.Size = new System.Drawing.Size(100, 100);
            this.picBusy.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.picBusy.TabIndex = 12;
            this.picBusy.TabStop = false;
            // 
            // formDoWork
            // 
            this.AcceptButton = this.btnSearch;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(836, 553);
            this.Controls.Add(this.picBusy);
            this.Controls.Add(this.chkSkip);
            this.Controls.Add(this.gridCurrentJob);
            this.Controls.Add(this.btnPrev);
            this.Controls.Add(this.btnNext);
            this.Controls.Add(this.btnJobs);
            this.Controls.Add(this.txtQuery);
            this.Controls.Add(this.btnSearch);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "formDoWork";
            this.Shown += new System.EventHandler(this.formDoWork_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.gridCurrentJob)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBusy)).EndInit();
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
        private System.Windows.Forms.CheckBox chkSkip;
        private TransparentPictureBox picBusy;
        private System.Windows.Forms.DataGridViewTextBoxColumn Parameter;
        private System.Windows.Forms.DataGridViewTextBoxColumn Value;


    }
}

