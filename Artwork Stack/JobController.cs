using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using TagLib;

namespace Artwork_Stack
{
    class JobController
    {
        public JobController()
        {
            Jobs = new DataSet();
            Jobs.Tables.Add("Tracks");
            Jobs.Tables["Tracks"].Columns.Add("Artist");
            Jobs.Tables["Tracks"].Columns.Add("Title");
            Jobs.Tables["Tracks"].Columns.Add("Album");
            Jobs.Tables["Tracks"].Columns.Add("Path");
        }
        public DataSet Jobs;
        public void TraverseFolder(string path) // TODO: add recurse subdir traversing
        {
            foreach (string f in Directory.GetFiles(path))
            {
                var track    = TagLib.File.Create(f);
                DataRow dr   = Jobs.Tables["Tracks"].Rows.Add();
                dr["Artist"] = track.Tag.FirstPerformer;
                dr["Title"]  = track.Tag.Title;
                dr["Album"]  = track.Tag.Album;
                dr["Path"]   = f;
            }
        }
        public void ShowJobList()
        {
            var fJobs = new formJobs();
            fJobs.gridJobs.DataSource = Jobs.Tables["Tracks"];
            fJobs.ShowDialog();
        }
    }
}
