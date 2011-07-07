using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Artwork_Stack
{
    public partial class formSetup : Form
    {
        public formSetup()
        {
            InitializeComponent();
        }

        private void btnBrowse_Click(object sender, EventArgs e) //TODO: Sync txt input and dialog
        {
            folderBrowse.ShowDialog();
            txtPath.Text = folderBrowse.SelectedPath;
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            //TODO: validate path
            (new formDoWork(txtPath.Text)).Show();
            this.Visible = false;
        }
    }
}
