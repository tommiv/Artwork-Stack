using System;
using System.ComponentModel;
using System.Drawing;
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
                
                var bw = new BackgroundWorker();

                var jcon = new JobController(path, chkGroup.Checked, chkRecurse.Checked, chkSkipExistingArt.Checked);
                jcon.WorkerInstance = bw;

                var panel = new Panel();
                panel.Size = this.Size;
                panel.Location = new Point(0, 0);
                this.Controls.Add(panel);
                panel.BringToFront();

                var message = new Label();
                message.TextAlign = ContentAlignment.MiddleCenter;
                message.Location = new Point(0, 0);
                message.Size = this.Size;
                this.Controls.Add(message);
                message.BringToFront();

                bw.DoWork += jcon.FillFilesStack;
                bw.WorkerReportsProgress = true;
                bw.ProgressChanged += (o, args) => message.Text = Verbal.FallThrough + Environment.NewLine + args;
                bw.RunWorkerAsync();
                while (bw.IsBusy)
                {
                    Application.DoEvents();
                }

                message.Location = new Point(0, -30);

                var pg = new ProgressBar();
                pg.Maximum = 100;
                pg.Size = new Size(this.Width - 30, 15);
                pg.Location = new Point(15, this.Height / 2 - 8);
                this.Controls.Add(pg);
                pg.BringToFront();

                bw = new BackgroundWorker();
                jcon.WorkerInstance = bw;
                bw.WorkerReportsProgress = true;
                bw.DoWork += jcon.GatherJobs;
                bw.ProgressChanged += (o, args) => { pg.Value = args.ProgressPercentage; message.Text = (string)args.UserState; };
                bw.RunWorkerAsync();
                while (bw.IsBusy)
                {
                    Application.DoEvents();
                }

                this.Controls.Remove(pg);
                this.Controls.Remove(message);
                this.Controls.Remove(panel);

                if (jcon.JobsCount > 0)
                {
                    (new Thread(() => (new DoWork(jcon)).ShowDialog())).Start();
                    this.Close();
                }
                else
                {
                    MessageBox.Show(Verbal.AllFilesFiltered);
                }
            }
            else MessageBox.Show(Verbal.DirNotExists);
        }
    }
}
