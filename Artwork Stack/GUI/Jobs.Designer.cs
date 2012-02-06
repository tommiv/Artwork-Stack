namespace Artwork_Stack.GUI
{
    partial class Jobs
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
            this.gridJobs = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.gridJobs)).BeginInit();
            this.SuspendLayout();
            // 
            // gridJobs
            // 
            this.gridJobs.AllowUserToAddRows = false;
            this.gridJobs.AllowUserToDeleteRows = false;
            this.gridJobs.AllowUserToResizeColumns = false;
            this.gridJobs.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.Lavender;
            this.gridJobs.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.gridJobs.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridJobs.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.gridJobs.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridJobs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridJobs.Location = new System.Drawing.Point(0, 0);
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
            this.gridJobs.Size = new System.Drawing.Size(826, 216);
            this.gridJobs.TabIndex = 0;
            this.gridJobs.DoubleClick += new System.EventHandler(this.gridJobs_DoubleClick);
            // 
            // Jobs
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(826, 216);
            this.Controls.Add(this.gridJobs);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(842, 500);
            this.MinimumSize = new System.Drawing.Size(842, 100);
            this.Name = "Jobs";
            this.Text = "Jobs list";
            ((System.ComponentModel.ISupportInitialize)(this.gridJobs)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.DataGridView gridJobs;

    }
}