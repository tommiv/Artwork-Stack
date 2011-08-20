using System.Data;
using System.IO;
using System.Linq;

namespace Artwork_Stack
{
    class JobController
    {
        public JobController()
        {
            Jobs = new DataSet();
            Jobs.Tables.Add("Tracks");
            Jobs.Tables["Tracks"].Columns.Add("ID",      typeof(int));
            Jobs.Tables["Tracks"].Columns.Add("Artist",  typeof(string));
            Jobs.Tables["Tracks"].Columns.Add("Title",   typeof(string));
            Jobs.Tables["Tracks"].Columns.Add("Album",   typeof(string));
            Jobs.Tables["Tracks"].Columns.Add("Path",    typeof(string));
            Jobs.Tables["Tracks"].Columns.Add("Done",    typeof(bool));
            Jobs.Tables["Tracks"].Columns.Add("Process", typeof(bool));
        }
        public DataSet Jobs;
        public void TraverseFolder(string path, bool recurse)
        {
            int jobID = TopMargin == 0 ? 0 : TopMargin + 1;
            foreach (string f in Directory.GetFiles(path))
            {
                if (!f.ToLowerInvariant().EndsWith(".mp3")) continue;

                var track     = TagLib.File.Create(f);
                DataRow dr    = Jobs.Tables["Tracks"].Rows.Add();
                dr["ID"]      = jobID;
                dr["Artist"]  = track.Tag.FirstPerformer;
                dr["Title"]   = track.Tag.Title;
                dr["Album"]   = track.Tag.Album;
                dr["Path"]    = f;
                dr["Done"]    = false;
                dr["Process"] = false;
                jobID++;
            }
            if (recurse)
                foreach (string folder in Directory.GetDirectories(path))
                    TraverseFolder(folder, true);
        }
        public void ShowJobList()
        {
            var fJobs = new formJobs();
            fJobs.gridJobs.DataSource = Jobs.Tables["Tracks"];
            fJobs.Show();
        }
        public string CreateQueryString(int jobID)
        {
            DataRow dr = Jobs.Tables["Tracks"].Rows[jobID];
            return dr["Artist"] + " " + dr["Album"];
        }
        public int PendingJobsCount
        {
            get { return Jobs.Tables["Tracks"].AsEnumerable().Where(r => (bool)r["Done"] == false).Count(); }
        }
        public int ProcessedJobsCount
        {
            get { return Jobs.Tables["Tracks"].AsEnumerable().Where(r => (bool)r["Process"]).Count(); }
        }
        public int CompletedJobsCount
        {
            get { return Jobs.Tables["Tracks"].AsEnumerable().Where(r => (bool)r["Done"]).Count(); }
        }
        public bool IsUnprocessedJobs
        {
            get { return Jobs.Tables["Tracks"].AsEnumerable().Where(r => (bool)r["done"] == false && (bool)r["Process"] == false).Count() > 0; }
        }
        public int JobsCount
        {
            get { return Jobs.Tables["Tracks"].Rows.Count; }
        }
        public int TopMargin
        {
            get
            {
                var lastNotDone = Jobs.Tables["Tracks"].AsEnumerable().LastOrDefault(r => (bool)r["Done"] == false);
                if (lastNotDone == null) return 0;
                else return (int)lastNotDone["ID"];
            }
        }
        public int BottomMargin
        {
            get
            {
                var firstNotDone = Jobs.Tables["Tracks"].AsEnumerable().FirstOrDefault(r => (bool)r["Done"] == false);
                if (firstNotDone == null) return 0;
                else return (int)firstNotDone["ID"];
            }
        }
        public void SetJobIsDone(int jobID)
        {
            DataRow dr = Jobs.Tables["Tracks"].Rows[jobID];
            dr["Process"] = false;
            dr["Done"]    = true;
        }
    }
}
