using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Artwork_Stack
{
    public class JobController
    {
        public  string    RootFolder;
        public  bool      Recurse;
        public  bool      Group;
        public  DataSet   Jobs;
        private DataTable T;
        private formJobs  fJobs;

        public JobController(string rootfolder, bool group, bool recurse)
        {
            RootFolder = rootfolder;
            Recurse    = recurse;
            Group      = group;

            Jobs = new DataSet();
            T = Jobs.Tables.Add("Tracks");
            T.Columns.Add("ID",       typeof(int));
            T.Columns.Add("Artist",   typeof(string));
            T.Columns.Add("Title",    typeof(string));
            T.Columns.Add("Album",    typeof(string));
            T.Columns.Add("Path",     typeof(string));
            T.Columns.Add("PathList", typeof(List<string>));
            T.Columns.Add("Done",     typeof(bool));
            T.Columns.Add("Process",  typeof(bool));
        }
        
        public void TraverseFolder()
        {
            GatherFiles(RootFolder, Recurse);
            
            if (!Group) return;
            var grouped = T.AsEnumerable().GroupBy(r => string.Format("{0}|{1}", r["Artist"], r["album"]));
            var nT = T.Clone();
            int jobid = 0;
            foreach (var group in grouped)
            {
                var data      = group.ToList();
                DataRow dr    = nT.Rows.Add();
                dr["ID"]      = jobid++;
                dr["Artist"]  = data[0]["Artist"];
                dr["Title"]   = data[0]["Title"];
                dr["Album"]   = data[0]["Album"];
                dr["Path"]    = null;
                dr["Done"]    = false;
                dr["Process"] = false;

                var pathList = new List<string>();
                foreach (var row in data) pathList.Add((string)row["path"]);
                dr["PathList"] = pathList;
            }
            Jobs.Tables.Remove("Tracks");
            Jobs.Tables.Add(nT.Copy());
            nT.Clear();

            T = Jobs.Tables["Tracks"];
        }

        private void GatherFiles(string path, bool recurse)
        {
            int jobID = TopMargin == 0 ? 0 : TopMargin + 1;
            foreach (string f in Directory.GetFiles(path))
            {
                if (!f.ToLowerInvariant().EndsWith(".mp3")) continue;

                var track     = TagLib.File.Create(f);
                DataRow dr    = T.Rows.Add();
                dr["ID"]      = jobID++;
                dr["Artist"]  = track.Tag.FirstPerformer;
                dr["Title"]   = track.Tag.Title;
                dr["Album"]   = track.Tag.Album;
                dr["Path"]    = f;
                dr["Done"]    = false;
                dr["Process"] = false;
            }
            if (recurse)
                foreach (string folder in Directory.GetDirectories(path))
                    GatherFiles(folder, true);
        }

        public formJobs ShowJobList()
        {
            if (fJobs == null || fJobs.IsDisposed)
            {
                fJobs = new formJobs();
                fJobs.gridJobs.DataSource = Jobs.Tables["Tracks"];
                // ReSharper disable PossibleNullReferenceException
                fJobs.gridJobs.Columns["Path"].Visible  = false;
                fJobs.gridJobs.Columns["ID"].Width      = 40;
                fJobs.gridJobs.Columns["Artist"].Width  = 220;
                fJobs.gridJobs.Columns["Title"].Width   = 220;
                fJobs.gridJobs.Columns["Album"].Width   = 220;
                fJobs.gridJobs.Columns["Done"].Width    = 40;
                fJobs.gridJobs.Columns["Process"].Width = 60;
                // ReSharper restore PossibleNullReferenceException
                fJobs.Show();
                foreach (DataGridViewRow row in fJobs.gridJobs.Rows)
                    if (string.IsNullOrEmpty(row.Cells["path"].Value.ToString()))
                        foreach (DataGridViewCell cell in row.Cells)
                            cell.Style.BackColor = row.Index % 2 == 0 ? Color.FromArgb(255, 240, 210, 240) : Color.FromArgb(255, 255, 220, 255);
            }
            return fJobs;
        }
        public string CreateQueryString(int jobID)
        {
            DataRow dr = T.Rows[jobID];
            return dr["Artist"] + " " + dr["Album"];
        }
        public int PendingJobsCount
        {
            get { return T.AsEnumerable().Where(r => (bool)r["Done"] == false).Count(); }
        }
        public int ProcessedJobsCount
        {
            get { return T.AsEnumerable().Where(r => (bool)r["Process"]).Count(); }
        }
        public int CompletedJobsCount
        {
            get { return T.AsEnumerable().Where(r => (bool)r["Done"]).Count(); }
        }
        public bool IsUnprocessedJobs
        {
            get { return T.AsEnumerable().Where(r => (bool)r["done"] == false && (bool)r["Process"] == false).Count() > 0; }
        }
        public int JobsCount
        {
            get { return T.Rows.Count; }
        }
        public int TopMargin
        {
            get
            {
                var lastNotDone = T.AsEnumerable().LastOrDefault(r => (bool)r["Done"] == false);
                if (lastNotDone == null) return 0;
                else return (int)lastNotDone["ID"];
            }
        }
        public int BottomMargin
        {
            get
            {
                var firstNotDone = T.AsEnumerable().FirstOrDefault(r => (bool)r["Done"] == false);
                if (firstNotDone == null) return 0;
                else return (int)firstNotDone["ID"];
            }
        }
        public void SetJobIsDone(int jobID)
        {
            DataRow dr = T.Rows[jobID];
            dr["Process"] = false;
            dr["Done"]    = true;
        }
    }
}
