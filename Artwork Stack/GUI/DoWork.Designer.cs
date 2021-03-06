﻿using Artwork_Stack.Controls;

namespace Artwork_Stack.GUI
{
    partial class DoWork
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.btnSearch = new System.Windows.Forms.Button();
            this.txtQuery = new System.Windows.Forms.TextBox();
            this.btnNext = new System.Windows.Forms.Button();
            this.btnPrev = new System.Windows.Forms.Button();
            this.gridCurrentJob = new System.Windows.Forms.DataGridView();
            this.Parameter = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Value = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.chkSkip = new System.Windows.Forms.CheckBox();
            this.btnJobs = new System.Windows.Forms.CheckBox();
            this.chkCrop = new System.Windows.Forms.CheckBox();
            this.chkResize = new System.Windows.Forms.CheckBox();
            this.numSize = new System.Windows.Forms.NumericUpDown();
            this.Sources = new System.Windows.Forms.TabControl();
            this.grpOptions = new System.Windows.Forms.GroupBox();
            this.gridJobs = new System.Windows.Forms.DataGridView();
            this.lblNotes = new System.Windows.Forms.Label();
            this.btnEncode = new System.Windows.Forms.Button();
            this.Busy = new Artwork_Stack.Controls.TransparentPictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.gridCurrentJob)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSize)).BeginInit();
            this.grpOptions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridJobs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Busy)).BeginInit();
            this.SuspendLayout();
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(861, 40);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(99, 23);
            this.btnSearch.TabIndex = 0;
            this.btnSearch.Text = "Override Search";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnOverrideSearch_Click);
            // 
            // txtQuery
            // 
            this.txtQuery.Location = new System.Drawing.Point(638, 14);
            this.txtQuery.Name = "txtQuery";
            this.txtQuery.Size = new System.Drawing.Size(322, 20);
            this.txtQuery.TabIndex = 3;
            // 
            // btnNext
            // 
            this.btnNext.Location = new System.Drawing.Point(885, 614);
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
            this.btnPrev.Location = new System.Drawing.Point(638, 614);
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
            this.gridCurrentJob.Location = new System.Drawing.Point(638, 84);
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
            // chkSkip
            // 
            this.chkSkip.Appearance = System.Windows.Forms.Appearance.Button;
            this.chkSkip.Location = new System.Drawing.Point(885, 574);
            this.chkSkip.Name = "chkSkip";
            this.chkSkip.Size = new System.Drawing.Size(75, 24);
            this.chkSkip.TabIndex = 11;
            this.chkSkip.Text = "Skip";
            this.chkSkip.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.chkSkip.UseVisualStyleBackColor = true;
            // 
            // btnJobs
            // 
            this.btnJobs.Appearance = System.Windows.Forms.Appearance.Button;
            this.btnJobs.Location = new System.Drawing.Point(759, 614);
            this.btnJobs.Name = "btnJobs";
            this.btnJobs.Size = new System.Drawing.Size(86, 24);
            this.btnJobs.TabIndex = 13;
            this.btnJobs.Text = "Jobs list";
            this.btnJobs.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnJobs.UseVisualStyleBackColor = true;
            this.btnJobs.CheckedChanged += new System.EventHandler(this.btnJobs_CheckedChanged);
            // 
            // chkCrop
            // 
            this.chkCrop.AutoSize = true;
            this.chkCrop.Checked = true;
            this.chkCrop.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkCrop.Location = new System.Drawing.Point(6, 19);
            this.chkCrop.Name = "chkCrop";
            this.chkCrop.Size = new System.Drawing.Size(95, 17);
            this.chkCrop.TabIndex = 14;
            this.chkCrop.Text = "Crop to square";
            this.chkCrop.UseVisualStyleBackColor = true;
            this.chkCrop.CheckedChanged += new System.EventHandler(this.chkCrop_CheckedChanged);
            // 
            // chkResize
            // 
            this.chkResize.AutoSize = true;
            this.chkResize.Location = new System.Drawing.Point(6, 42);
            this.chkResize.Name = "chkResize";
            this.chkResize.Size = new System.Drawing.Size(109, 17);
            this.chkResize.TabIndex = 15;
            this.chkResize.Text = "Resize big img to:";
            this.chkResize.UseVisualStyleBackColor = true;
            this.chkResize.CheckedChanged += new System.EventHandler(this.chkResize_CheckedChanged);
            // 
            // numSize
            // 
            this.numSize.Location = new System.Drawing.Point(121, 40);
            this.numSize.Maximum = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            this.numSize.Minimum = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.numSize.Name = "numSize";
            this.numSize.Size = new System.Drawing.Size(59, 20);
            this.numSize.TabIndex = 16;
            this.numSize.Value = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.numSize.ValueChanged += new System.EventHandler(this.numSize_ValueChanged);
            // 
            // Sources
            // 
            this.Sources.Location = new System.Drawing.Point(12, 12);
            this.Sources.Name = "Sources";
            this.Sources.SelectedIndex = 0;
            this.Sources.Size = new System.Drawing.Size(610, 626);
            this.Sources.TabIndex = 17;
            // 
            // grpOptions
            // 
            this.grpOptions.Controls.Add(this.chkCrop);
            this.grpOptions.Controls.Add(this.chkResize);
            this.grpOptions.Controls.Add(this.numSize);
            this.grpOptions.Location = new System.Drawing.Point(638, 529);
            this.grpOptions.Name = "grpOptions";
            this.grpOptions.Size = new System.Drawing.Size(228, 69);
            this.grpOptions.TabIndex = 18;
            this.grpOptions.TabStop = false;
            this.grpOptions.Text = "Options";
            // 
            // gridJobs
            // 
            this.gridJobs.AllowUserToAddRows = false;
            this.gridJobs.AllowUserToDeleteRows = false;
            this.gridJobs.AllowUserToResizeColumns = false;
            this.gridJobs.AllowUserToResizeRows = false;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.Lavender;
            this.gridJobs.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle3;
            this.gridJobs.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridJobs.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.gridJobs.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.gridJobs.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridJobs.Location = new System.Drawing.Point(12, 654);
            this.gridJobs.MultiSelect = false;
            this.gridJobs.Name = "gridJobs";
            this.gridJobs.ReadOnly = true;
            this.gridJobs.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.gridJobs.RowHeadersVisible = false;
            this.gridJobs.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.gridJobs.RowTemplate.Height = 20;
            this.gridJobs.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.gridJobs.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridJobs.ShowCellErrors = false;
            this.gridJobs.ShowEditingIcon = false;
            this.gridJobs.ShowRowErrors = false;
            this.gridJobs.Size = new System.Drawing.Size(948, 216);
            this.gridJobs.TabIndex = 20;
            this.gridJobs.CellContentDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridJobs_CellContentDoubleClick);
            // 
            // lblNotes
            // 
            this.lblNotes.AutoSize = true;
            this.lblNotes.Location = new System.Drawing.Point(635, 294);
            this.lblNotes.Name = "lblNotes";
            this.lblNotes.Size = new System.Drawing.Size(147, 13);
            this.lblNotes.TabIndex = 22;
            this.lblNotes.Text = "Shortcuts and other stuff here";
            // 
            // btnEncode
            // 
            this.btnEncode.Location = new System.Drawing.Point(638, 40);
            this.btnEncode.Name = "btnEncode";
            this.btnEncode.Size = new System.Drawing.Size(115, 23);
            this.btnEncode.TabIndex = 23;
            this.btnEncode.Text = "WTF with encoding?";
            this.btnEncode.UseVisualStyleBackColor = true;
            this.btnEncode.Click += new System.EventHandler(this.btnEncode_Click);
            // 
            // Busy
            // 
            this.Busy.Image = global::Artwork_Stack.Properties.Resources.ajax_loader;
            this.Busy.Location = new System.Drawing.Point(450, 275);
            this.Busy.Name = "Busy";
            this.Busy.Size = new System.Drawing.Size(100, 100);
            this.Busy.TabIndex = 21;
            this.Busy.TabStop = false;
            // 
            // DoWork
            // 
            this.AcceptButton = this.btnSearch;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(974, 882);
            this.Controls.Add(this.btnEncode);
            this.Controls.Add(this.lblNotes);
            this.Controls.Add(this.Busy);
            this.Controls.Add(this.gridJobs);
            this.Controls.Add(this.grpOptions);
            this.Controls.Add(this.Sources);
            this.Controls.Add(this.btnJobs);
            this.Controls.Add(this.chkSkip);
            this.Controls.Add(this.gridCurrentJob);
            this.Controls.Add(this.btnPrev);
            this.Controls.Add(this.btnNext);
            this.Controls.Add(this.txtQuery);
            this.Controls.Add(this.btnSearch);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.Location = new System.Drawing.Point(50, 50);
            this.MaximizeBox = false;
            this.Name = "DoWork";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Artwork Stack: Multi-service artwork fetcher";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DoWork_FormClosing);
            this.Shown += new System.EventHandler(this.formDoWork_Shown);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.DoWork_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.gridCurrentJob)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSize)).EndInit();
            this.grpOptions.ResumeLayout(false);
            this.grpOptions.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridJobs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Busy)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.TextBox txtQuery;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.Button btnPrev;
        private System.Windows.Forms.DataGridView gridCurrentJob;
        private System.Windows.Forms.CheckBox chkSkip;
        private System.Windows.Forms.DataGridViewTextBoxColumn Parameter;
        private System.Windows.Forms.DataGridViewTextBoxColumn Value;
        private System.Windows.Forms.CheckBox btnJobs;
        private System.Windows.Forms.CheckBox chkCrop;
        private System.Windows.Forms.CheckBox chkResize;
        private System.Windows.Forms.NumericUpDown numSize;
        private System.Windows.Forms.TabControl Sources;
        private System.Windows.Forms.GroupBox grpOptions;
        private imageCell cellEmbeded;
        public System.Windows.Forms.DataGridView gridJobs;
        private TransparentPictureBox Busy;
        private System.Windows.Forms.Label lblNotes;
        private System.Windows.Forms.Button btnEncode;
    }
}

