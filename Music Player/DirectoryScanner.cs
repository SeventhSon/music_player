using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Collections.ObjectModel;
using System.Data;
namespace Music_Player
{
    class DirectoryScanner
    {
        private static volatile DirectoryScanner instance;
        private static object monitor = new Object();
        private Collection <FileSystemWatcher> FSW;
        private DirectoryScanner()
        {
            FSW = new Collection<FileSystemWatcher>();
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public DataTable Scan(string path)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Title", typeof(string));
            dt.Columns.Add("Artist", typeof(string));
            dt.Columns.Add("Album", typeof(string));
            dt.Columns.Add("Genre", typeof(string));
            dt.Columns.Add("Length", typeof(int));
            dt.Columns.Add("DirectoryID", typeof(int));
            dt.Columns.Add("Path", typeof(string));
            DirectoryInfo DI = new DirectoryInfo(path);
            FileSystemWatcher watcher = new FileSystemWatcher();
            watcher.Path = DI.FullName;
            FSW.Add(watcher);
            DBManager dbm = DBManager.Instance;
            dbm.executeNonQuery("Insert or replace into directories (path, last_write_time) values ('" + DI.FullName + "'," + File.GetLastWriteTime(DI.FullName).ToFileTime() + ")");
            int dirid = Int32.Parse(((dbm.executeQuery("Select id from directories where path='" + DI.FullName + "'")).Rows[0]["id"]).ToString());
            foreach (FileInfo file in DI.GetFiles())
            {
                if(file.Extension == ".mp3")
                {
                    TagLib.File tags = TagLib.File.Create(file.FullName);
                    dt.Rows.Add(tags.Tag.Title, ConvertStringArrayToString(tags.Tag.AlbumArtists), tags.Tag.Album, ConvertStringArrayToString(tags.Tag.Genres), tags.Properties.Duration.TotalSeconds, dirid,file.FullName);
                }
            }
            return dt;
        }
        /// <summary>
        /// Scans directory given by path. Descends into subdirectories and recursively scan them. Inserts scanned directories in DB
        /// </summary>
        /// <param name="path">Location of the directory</param>
        /// <param name="dt">DataTable handle for recursive addition</param>
        /// <returns>DataTable with scanned mp3 files</returns>
        public DataTable ScanRecursive(string path, DataTable dt)
        {
            if(dt == null)
            {
                dt = new DataTable();
                dt.Columns.Add("Title", typeof(string));
                dt.Columns.Add("Artist", typeof(string));
                dt.Columns.Add("Album", typeof(string));
                dt.Columns.Add("Genre", typeof(string));
                dt.Columns.Add("Length", typeof(int));
                dt.Columns.Add("DirectoryID", typeof(int));
                dt.Columns.Add("Path", typeof(string));
            }
            DirectoryInfo DI = new DirectoryInfo(path);
            FileSystemWatcher watcher = new FileSystemWatcher();
            watcher.Path = DI.FullName;
            FSW.Add(watcher);
            DBManager dbm = DBManager.Instance;
            dbm.executeNonQuery("Insert or replace into directories (path, last_write_time) values ('" + DI.FullName + "'," + Directory.GetLastWriteTime(DI.FullName).ToFileTime() + ")");
            int dirid = Int32.Parse(((dbm.executeQuery("Select id from directories where path='" + DI.FullName + "'")).Rows[0]["id"]).ToString());
            if (!hasAccessToFolder(path)) return dt;
            foreach (FileInfo file in DI.GetFiles())
            {
                if(file.Extension == ".mp3")
                {
                    TagLib.File tags = TagLib.File.Create(file.FullName);
                    dt.Rows.Add(tags.Tag.Title, ConvertStringArrayToString(tags.Tag.AlbumArtists), tags.Tag.Album, ConvertStringArrayToString(tags.Tag.Genres), tags.Properties.Duration.TotalSeconds, dirid,file.FullName);
                }
            }
            DirectoryInfo[] subDirectories = DI.GetDirectories();

            foreach (DirectoryInfo subDirectory in subDirectories)
                ScanRecursive(subDirectory.FullName,dt);
            return dt;
        }
        private bool hasAccessToFolder(string folderPath)
        {
            try
            {
                DirectoryInfo DI = new DirectoryInfo(folderPath);
                DI.GetFiles();
                return true;
            }
            catch (UnauthorizedAccessException)
            {
                return false;
            }
        }
        /// <summary>
        /// Constructs a string delimited by comas from given string array
        /// </summary>
        /// <param name="array">Array of strings</param>
        /// <returns>Concatanation of strings</returns>
        static string ConvertStringArrayToString(string[] array)
        {
            //
            // Concatenate all the elements into a StringBuilder.
            //
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < array.Length; i++)
            {
                builder.Append(array[i]);
                if(i!=array.Length-1)
                    builder.Append(',');
            }
            return builder.ToString();
        }
    }
}
