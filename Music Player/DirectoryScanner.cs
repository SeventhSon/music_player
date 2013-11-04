using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Collections.ObjectModel;

namespace Music_Player
{
    class DirectoryScanner
    {
        private static volatile DirectoryScanner instance;
        private static object monitor = new Object();
        private Collection <FileSystemWatcher> FSW;
        private DirectoryScanner()
        {
            //Create a new FileSystemWatcher.
            FileSystemWatcher watcher = new FileSystemWatcher();

            //Set the filter to only catch TXT files.
            watcher.Filter = "*.mp3";

            //Subscribe to the Created event.
            watcher.Created += new FileSystemEventHandler(watcher_FileCreated);

            //Set the path
            //////////////////////////some problems here
            //watcher.Path = path;

            //Enable the FileSystemWatcher events.
            watcher.EnableRaisingEvents = true;
        }

        void watcher_FileCreated(object sender, FileSystemEventArgs e)
        {
            //scanning path again

        }
        public static DirectoryScanner Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (monitor)
                    {
                        if (instance == null)
                            instance = new DirectoryScanner();
                    }
                }
                return instance;
            }
        }
        public int Scan(string path)
        {
            int MusicFilesFound = 0;
            DirectoryInfo DI = new DirectoryInfo(path);
            FileSystemWatcher watcher = new FileSystemWatcher();
            watcher.Path = DI.FullName;
            FSW.Add(watcher);
            foreach (FileInfo file in DI.GetFiles())
            {
                if(file.Extension == ".mp3")
                {
                    MusicFilesFound++;
                    //adding file do database
                }
            }
            //After scanning add directory to FileSystemWatcher
            return MusicFilesFound;
        }
        public int ScanRecursive(string path)
        {
            int MusicFilesFound = 0;
            DirectoryInfo DI = new DirectoryInfo(path);
            FileSystemWatcher watcher = new FileSystemWatcher();
            watcher.Path = DI.FullName;
            FSW.Add(watcher);
            foreach (FileInfo file in DI.GetFiles())
            {
                if (file.Extension == ".mp3")
                {
                    MusicFilesFound++;
                    //adding file to database
                }
            }
            DirectoryInfo[] subDirectories = DI.GetDirectories();

            foreach (DirectoryInfo subDirectory in subDirectories)
            {
                watcher.Path = subDirectory.FullName;
                FSW.Add(watcher);
                ScanRecursive(subDirectory.FullName);
            }
            //After scanning add all directories to FileSystemWatcher
            return MusicFilesFound;
        }
        public int GetLastWriteTime(string path)
        {
            int LastWriteTime = -1;
            return LastWriteTime;
        }
        private static void OnFileChange()
        {
            //On change use DBManager to send infromation
            //Later notify LibraryManager to refresh
        }
    }
}
