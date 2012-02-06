using System;
using System.Threading;
using System.Windows.Forms;
using System.IO;
using Artwork_Stack.Tools;

namespace Artwork_Stack.GUI
{
    public partial class Setup : Form
    {
        public Setup() { InitializeComponent(); }

        private void formSetup_Load(object sender, EventArgs e)
        {
            txtPath.Text               = WinRegistry.GetValue<string>(WinRegistry.Keys.DefaultPath);
            chkRecurse.Checked         = WinRegistry.GetValue<bool>  (WinRegistry.Keys.RecurseTraversing);
            chkGroup.Checked           = WinRegistry.GetValue<bool>  (WinRegistry.Keys.GroupByAlbum);
            chkSkipExistingArt.Checked = WinRegistry.GetValue<bool>  (WinRegistry.Keys.SkipExisting);
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            folderBrowse.SelectedPath = txtPath.Text;
            folderBrowse.ShowDialog();
            txtPath.Text = folderBrowse.SelectedPath;
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            string path = txtPath.Text;
            if (Directory.Exists(path))
            {
                WinRegistry.SetValue(WinRegistry.Keys.DefaultPath,       path);
                WinRegistry.SetValue(WinRegistry.Keys.GroupByAlbum,      chkGroup.Checked);
                WinRegistry.SetValue(WinRegistry.Keys.RecurseTraversing, chkRecurse.Checked);
                WinRegistry.SetValue(WinRegistry.Keys.SkipExisting,      chkSkipExistingArt.Checked);
                var jcon = new JobController(path, chkGroup.Checked, chkRecurse.Checked, chkSkipExistingArt.Checked);
                (new Thread(() => (new DoWork(jcon)).ShowDialog())).Start();
                this.Close();
            }
            else MessageBox.Show(@"Directory not exists");
        }
    }
}
