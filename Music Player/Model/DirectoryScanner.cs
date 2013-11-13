using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Collections.ObjectModel;
using System.Data;
namespace Music_Player.Model
{
    class DirectoryScanner
    {
        private Collection <FileSystemWatcher> FSW;
        public DirectoryScanner()
        {
            FSW = new Collection<FileSystemWatcher>();
        }

        void OnFileChanged(object sender, FileSystemEventArgs e)
        {
            //scanning path again
            
        }
        /// <summary>
        /// Scans directory given by path. Descends into subdirectories and recursively scan them. Inserts scanned directories in DB
        /// </summary>
        /// <param name="path">Location of the directory</param>
        /// <param name="dt">DataTable handle for recursive addition</param>
        /// <returns>DataTable with scanned mp3 files</returns>
        public void ScanRecursive(string path,int dirId = -1)
        {
            if (!hasAccessToFolder(path)) 
                return;
            
            DirectoryInfo DI = new DirectoryInfo(path);
            DBManager dbm = DBManager.Instance;
            FileSystemWatcher watcher = new FileSystemWatcher(DI.FullName);
            watcher.Changed += OnFileChanged;
            FSW.Add(watcher);

            if(dirId == -1)
            {
                dbm.ExecuteNonQuery("Insert or replace into directories (path, last_write_time) values ('" + DI.FullName + "'," + Directory.GetLastWriteTime(DI.FullName).ToFileTime() + ")");
                dirId = Int32.Parse(((dbm.ExecuteQuery("Select id from directories where path='" + DI.FullName + "'")).Rows[0]["id"]).ToString());
            }
            string insertString = "";
            int i = 0;
            foreach (FileInfo file in DI.GetFiles())
            {
                if(file.Extension == ".mp3")
                {
                    if (i > 0)
                        insertString += ", ";
                    TagLib.File tags = TagLib.File.Create(file.FullName);
                    insertString += " (\""+tags.Tag.Title+"\", \""+ ConvertStringArrayToString(tags.Tag.AlbumArtists)+"\", \""+tags.Tag.Album+"\", \""+ConvertStringArrayToString(tags.Tag.Genres)+"\", "+tags.Properties.Duration.TotalSeconds+", "+dirId+", \""+file.FullName+"\", "+tags.Tag.Track+", "+tags.Tag.Year+") ";
                    i++;
                }    
            }
            if (!insertString.Equals(""))
                dbm.ExecuteNonQuery("Insert or replace into songs (title, artist, album, genre, length, id_directory, path, track_no, year) values" + insertString);
            DirectoryInfo[] subDirectories = DI.GetDirectories();

            foreach (DirectoryInfo subDirectory in subDirectories)
                ScanRecursive(subDirectory.FullName,dirId);
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
        public void RescanAll(DataTable dirs)
        {
            foreach (DataRow row in dirs.Rows)
            {
                string dirId = row["ID"].ToString();
                string path = row["Path"].ToString();
                long lastWriteTime = (long)row["LastWriteTime"];
                try
                {
                    DBManager dbm = DBManager.Instance;
                    if (!Directory.Exists(path))
                    {
                        dbm.ExecuteNonQuery("DELETE FROM directories WHERE id=" + dirId);
                        dbm.ExecuteNonQuery("DELETE FROM songs WHERE id_directory=" + dirId);
                    }
                    else if (lastWriteTime < Directory.GetLastWriteTime(path).ToFileTime())
                    {
                        dbm.ExecuteNonQuery("DELETE FROM songs WHERE id_directory=" + dirId);
                        ScanRecursive(path);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }

        public void ForceBroadcastDirectories()
        {
            DBManager dbm = DBManager.Instance;
            DataTable dt = dbm.ExecuteQuery("select * from directories");
            List<DirectoryModel> packet = new List<DirectoryModel>();
            foreach(DataRow row in dt.Rows)
            {
                DirectoryModel dir = new DirectoryModel();
                dir.Path = row["path"].ToString();
                dir.LastWrite = (long)row["last_write_time"];
                dir.NoRemove = true;
                dir.Id = Int32.Parse(row["id"].ToString());
                packet.Add(dir);
            }
            GalaSoft.MvvmLight.Messaging.Messenger.Default.Send<List<DirectoryModel>>(packet);
        }
    }
}
