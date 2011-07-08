using System.Data;
using System.IO;

namespace Artwork_Stack
{
    class JobController
    {
        public JobController()
        {
            Jobs = new DataSet();
            Jobs.Tables.Add("Tracks");
            Jobs.Tables["Tracks"].Columns.Add("ID");
            Jobs.Tables["Tracks"].Columns.Add("Artist");
            Jobs.Tables["Tracks"].Columns.Add("Title");
            Jobs.Tables["Tracks"].Columns.Add("Album");
            Jobs.Tables["Tracks"].Columns.Add("Path");
            Jobs.Tables["Tracks"].Columns.Add("Done");
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
        public int JobsCount
        {
            get { return Jobs.Tables["Tracks"].Rows.Count; }
        }
    }
}
