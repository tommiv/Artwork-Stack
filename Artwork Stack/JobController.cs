using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Artwork_Stack
{
    public class JobController
    {
        public  DataSet   Jobs;
        private DataTable T;

        public BackgroundWorker WorkerInstance;
        private readonly string Path;
        private readonly bool   Group;
        private readonly bool   Recurse;
        private readonly bool   Skip;

        public JobController(string path, bool group, bool recurse, bool skip)
        {
            this.Path    = path;
            this.Group   = group;
            this.Recurse = recurse;
            this.Skip    = skip;

            Jobs = new DataSet();
            T = Jobs.Tables.Add(Fields.Tracks);
            T.Columns.Add(Fields.ID,       typeof(int));
            T.Columns.Add(Fields.Artist,   typeof(string));
            T.Columns.Add(Fields.Title,    typeof(string));
            T.Columns.Add(Fields.Album,    typeof(string));
            T.Columns.Add(Fields.Path,     typeof(string));
            T.Columns.Add(Fields.PathList, typeof(List<string>));
            T.Columns.Add(Fields.Done,     typeof(bool));
            T.Columns.Add(Fields.Process,  typeof(bool));
        }

        public void GatherJobs(object sender, DoWorkEventArgs doWorkEventArgs)
        {
            int i = 0;
            int c = FilesStack.Count;
            while (FilesStack.Count > 0)
            {
                string f = FilesStack.Pop();

                TagLib.File track = null;
                try
                {
                    track = TagLib.File.Create(f);
                }
                catch (Exception e)
                {
                    MessageBox.Show(@"Opening file error: " + e.Message);
                }

                if (track == null || (Skip && track.Tag.Pictures.GetLength(0) > 0)) continue;

                DataRow dr         = T.Rows.Add();
                dr[Fields.ID]      = i++;
                dr[Fields.Artist]  = track.Tag.FirstPerformer;
                dr[Fields.Title]   = track.Tag.Title;
                dr[Fields.Album]   = track.Tag.Album;
                dr[Fields.Path]    = f;
                dr[Fields.Done]    = false;
                dr[Fields.Process] = false;

                int progress = Math.Min((int)Math.Ceiling((double)i/c*100), 100);
                WorkerInstance.ReportProgress(progress, string.Format("{0}/{1}; {2}", i, c, f));
            }

            if (Group) EnableGrouping();
        }

        private void EnableGrouping()
        {
            var grouped = T.AsEnumerable().GroupBy(r => string.Format("{0}|{1}", r[Fields.Artist], r[Fields.Album]));
            var nT = T.Clone();
            int jobid = 0;
            foreach (var group in grouped)
            {
                var data = group.ToList();
                DataRow dr = nT.Rows.Add();
                dr[Fields.ID] = jobid++;
                dr[Fields.Artist] = data[0][Fields.Artist];
                dr[Fields.Title] = data[0][Fields.Title];
                dr[Fields.Album] = data[0][Fields.Album];
                dr[Fields.Path] = data.Count > 1 ? null : data[0][Fields.Path];
                dr[Fields.Done] = false;
                dr[Fields.Process] = false;

                if (data.Count <= 1) continue;

                var pathList = new List<string>();
                foreach (var row in data) pathList.Add((string)row[Fields.Path]);
                dr[Fields.PathList] = pathList;
            }
            Jobs.Tables.Remove(Fields.Tracks);
            Jobs.Tables.Add(nT.Copy());
            nT.Clear();

            T = Jobs.Tables[Fields.Tracks];
        }

        private readonly Stack<string> FilesStack = new Stack<string>();

        public void FillFilesStack(object sender, DoWorkEventArgs doWorkEventArgs)
        {
            FallThrough(Path, Recurse);
        }

        private void FallThrough(string path, bool recursive)
        {
            foreach (string f in Directory.GetFiles(path))
            {
                if (f.ToLowerInvariant().EndsWith(".mp3"))
                {
                    FilesStack.Push(f);
                    WorkerInstance.ReportProgress(0, "Added file " + f);
                }
            }
            if (recursive)
            {
                foreach (string folder in Directory.GetDirectories(path))
                {
                    FallThrough(folder, true);
                }
            }
        }

        public string CreateQueryString(int jobID)
        {
            DataRow dr = T.Rows[jobID];
            return dr[Fields.Artist] + " " + dr[Fields.Album];
        }
        public int PendingJobsCount
        {
            get { return T.AsEnumerable().Count(r => (bool)r[Fields.Done] == false); }
        }
        public int ProcessedJobsCount
        {
            get { return T.AsEnumerable().Count(r => (bool)r[Fields.Process]); }
        }
        public int CompletedJobsCount
        {
            get { return T.AsEnumerable().Count(r => (bool)r[Fields.Done]); }
        }
        public bool IsUnprocessedJobs
        {
            get { return T.AsEnumerable().Any(r => (bool)r[Fields.Done] == false && (bool)r[Fields.Process] == false); }
        }
        public int JobsCount
        {
            get { return T.Rows.Count; }
        }
        public int TopMargin
        {
            get
            {
                var lastNotDone = T.AsEnumerable().LastOrDefault(r => (bool)r[Fields.Done] == false);
                if (lastNotDone == null) return 0;
                else return (int)lastNotDone[Fields.ID];
            }
        }
        public int BottomMargin
        {
            get
            {
                var firstNotDone = T.AsEnumerable().FirstOrDefault(r => (bool)r[Fields.Done] == false);
                if (firstNotDone == null) return 0;
                else return (int)firstNotDone[Fields.ID];
            }
        }
        public void SetJobIsDone(int jobID)
        {
            DataRow dr = T.Rows[jobID];
            dr[Fields.Process] = false;
            dr[Fields.Done]    = true;
        }
    }

    public static class Fields
    {
        public const string ID       = "ID";
        public const string Artist   = "Artist";
        public const string Title    = "Title";
        public const string Album    = "Album";
        public const string Path     = "Path";
        public const string Done     = "Done";
        public const string Process  = "Process";
        public const string PathList = "PathList";
        public const string Tracks   = "Tracks";
    }
}
