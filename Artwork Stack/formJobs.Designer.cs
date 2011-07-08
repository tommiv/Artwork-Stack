namespace Artwork_Stack
{
    partial class formJobs
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
            this.gridJobs.AllowUserToOrderColumns = true;
            this.gridJobs.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.Lavender;
            this.gridJobs.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.gridJobs.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCellsExceptHeader;
            this.gridJobs.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.gridJobs.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridJobs.Dock = System.Windows.Forms.DockStyle.Top;
            this.gridJobs.Location = new System.Drawing.Point(0, 0);
            this.gridJobs.MultiSelect = false;
            this.gridJobs.Name = "gridJobs";
            this.gridJobs.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.gridJobs.RowHeadersVisible = false;
            this.gridJobs.RowTemplate.Height = 20;
            this.gridJobs.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridJobs.ShowCellErrors = false;
            this.gridJobs.ShowEditingIcon = false;
            this.gridJobs.ShowRowErrors = false;
            this.gridJobs.Size = new System.Drawing.Size(812, 517);
            this.gridJobs.TabIndex = 0;
            // 
            // formJobs
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(812, 541);
            this.Controls.Add(this.gridJobs);
            this.Name = "formJobs";
            this.Text = "formJobs";
            ((System.ComponentModel.ISupportInitialize)(this.gridJobs)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.DataGridView gridJobs;

    }
}