using System;
using System.Threading;
using System.Windows.Forms;

namespace Artwork_Stack
{
    public partial class formSetup : Form
    {
        public formSetup()
        {
            InitializeComponent();
        }

        private void btnBrowse_Click(object sender, EventArgs e) //TODO: get path from registry
        {
            folderBrowse.SelectedPath = txtPath.Text;
            folderBrowse.ShowDialog();
            txtPath.Text = folderBrowse.SelectedPath;
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            //TODO: validate path
            string path = txtPath.Text;
            (new Thread(() => (new formDoWork(path)).ShowDialog())).Start();
            this.Close();
        }
    }
}
