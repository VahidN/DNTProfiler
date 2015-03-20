using System;
using System.IO;

namespace DNTProfiler.Common.Toolkit
{
    public class DirectoryMonitor : FileSystemWatcher
    {
        public DirectoryMonitor(String inDirectoryPath)
            : base(inDirectoryPath)
        {
            init();
        }

        public DirectoryMonitor(String inDirectoryPath, string inFilter)
            : base(inDirectoryPath, inFilter)
        {
            init();
        }

        public Action<string> OnFileSystemChanged { set; get; }

        public void Watcher_Changed(object sender, FileSystemEventArgs inArgs)
        {
            if (OnFileSystemChanged != null)
                OnFileSystemChanged(inArgs.FullPath);
        }

        public void Watcher_Created(object source, FileSystemEventArgs inArgs)
        {
            if (OnFileSystemChanged != null)
                OnFileSystemChanged(inArgs.FullPath);
        }

        public void Watcher_Deleted(object sender, FileSystemEventArgs inArgs)
        {
            if (OnFileSystemChanged != null)
                OnFileSystemChanged(inArgs.FullPath);
        }

        public void Watcher_Renamed(object sender, RenamedEventArgs inArgs)
        {
            if (OnFileSystemChanged != null)
                OnFileSystemChanged(inArgs.FullPath);
        }

        private void init()
        {
            IncludeSubdirectories = false;
            NotifyFilter = NotifyFilters.FileName | NotifyFilters.Size;
            EnableRaisingEvents = true;
            Created += Watcher_Created;
            Changed += Watcher_Changed;
            Deleted += Watcher_Deleted;
            Renamed += Watcher_Renamed;
        }
    }
}