using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Music_Player
{
    class DirectoryScanner
    {
        private static volatile DirectoryScanner instance;
        private static object monitor = new Object();
        private FileSystemWatcher FSW;
        private DirectoryScanner()
        {
            //Initialize FileSystemWatcher
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
            //After scanning add directory to FileSystemWatcher
            return MusicFilesFound;
        }
        public int ScanRecursive(string path)
        {
            int MusicFilesFound = 0;
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
