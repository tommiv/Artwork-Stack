using System;
using System.Threading;
using System.Windows.Forms;
using Microsoft.Win32;
using System.IO;

namespace Artwork_Stack
{
    public partial class formSetup : Form
    {
        public formSetup()
        {
            InitializeComponent();
        }

        private void formSetup_Load(object sender, EventArgs e)
        {
            txtPath.Text       = (string)getRegValue(regKeys.DefaultPath);
            chkRecurse.Checked = Convert.ToBoolean(getRegValue(regKeys.RecurseTraversing));
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
            bool recurse = chkRecurse.Checked;
            if (Directory.Exists(path))
            {
                setRegValue(regKeys.DefaultPath,       path);
                setRegValue(regKeys.RecurseTraversing, recurse);
                (new Thread(() => (new formDoWork(path, recurse)).ShowDialog())).Start();
                this.Close();
            }
            else MessageBox.Show(@"Directory not exists");
        }

        private const string RegPath = @"Software\ArtworkStack";
        private static object getRegValue(string key)
        {
            RegistryKey HKLM = Registry.LocalMachine;
            Object value;
            try
            {
                value = HKLM.OpenSubKey(RegPath).GetValue(key);
            }
            catch
            {
                value = null;
            }
            HKLM.Close();
            return value;
        }

        private static void setRegValue(string key, object value)
        {
            RegistryKey HKLM = Registry.LocalMachine;
            RegistryKey Artwrk = null;
            try
            {
                Artwrk = HKLM.OpenSubKey(RegPath, true);
            }
            finally
            {
                if (Artwrk == null) Artwrk = HKLM.CreateSubKey(RegPath, RegistryKeyPermissionCheck.ReadWriteSubTree);
            }

            if (Artwrk != null)
            {
                Artwrk.SetValue(key, value);
                Artwrk.Close();
            }
            HKLM.Close();
        }
    }
    static class regKeys
    {
        public const string DefaultPath       = "DefaultPath";
        public const string RecurseTraversing = "RecurseTraversing";
    }
}
