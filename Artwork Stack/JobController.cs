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
            Jobs.Tables["Tracks"].Columns.Add("ID",     typeof(int));
            Jobs.Tables["Tracks"].Columns.Add("Artist", typeof(string));
            Jobs.Tables["Tracks"].Columns.Add("Title",  typeof(string));
            Jobs.Tables["Tracks"].Columns.Add("Album",  typeof(string));
            Jobs.Tables["Tracks"].Columns.Add("Path",   typeof(string));
            Jobs.Tables["Tracks"].Columns.Add("Done",   typeof(bool));
        }
        public DataSet Jobs;
        public void TraverseFolder(string path) // TODO: add recurse subdir traversing
        {
            int jobID = 0;
            foreach (string f in Directory.GetFiles(path))
            {
                var track    = TagLib.File.Create(f);
                DataRow dr   = Jobs.Tables["Tracks"].Rows.Add();
                dr["ID"]     = jobID;
                dr["Artist"] = track.Tag.FirstPerformer;
                dr["Title"]  = track.Tag.Title;
                dr["Album"]  = track.Tag.Album;
                dr["Path"]   = f;
                dr["Done"]   = false;
                jobID++;
            }
        }
        public void ShowJobList()
        {
            var fJobs = new formJobs();
            fJobs.gridJobs.DataSource = Jobs.Tables["Tracks"];
            fJobs.ShowDialog();
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
        public int JobsCount
        {
            get { return Jobs.Tables["Tracks"].Rows.Count; }
        }
        public int TopMargin
        {
            get { return (int)Jobs.Tables["Tracks"].AsEnumerable().Last(r => (bool)r["Done"] == false)["ID"]; }
        }
        public int BottomMargin
        {
            get { return (int)Jobs.Tables["Tracks"].AsEnumerable().First(r => (bool)r["Done"] == false)["ID"]; }
        }
        public void SetJobIsDone(int jobID)
        {
            DataRow dr = Jobs.Tables["Tracks"].Rows[jobID];
            dr["Done"] = true;
        }
    }
}
