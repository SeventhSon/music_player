using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using GalaSoft.MvvmLight.Messaging;
using Music_Player.ViewModel;
using System.IO;

namespace Music_Player.Model
{
    class LibraryManager
    {
        /// <summary>
        /// Create new object of Library Manager and rescans every already scanned directory
        /// </summary>
        public LibraryManager()
        {
            RescanLibrary();
        }
        /// <summary>
        /// Queries DB for songs data
        /// </summary>
        /// <returns>DataTable with songs info</returns>
        public DataTable GetSongs()
        {
            DBManager dbm = DBManager.Instance;
            return dbm.executeQuery("Select title as Title, artist as Artist, album as Album, genre as Genre, length as Time,path as Path from songs");
        }
        /// <summary>
        /// Queries DB for directories data
        /// </summary>
        /// <returns>DataTable with directiories info</returns>
        public DataTable GetDirectories()
        {
            DBManager dbm = DBManager.Instance;
            return dbm.executeQuery("Select id as ID, path as Path, last_write_time as LastWriteTime from directories");
        }
        /// <summary>
        /// Queries DB for songs data
        /// </summary>
        /// <returns>DataTable with playlists info</returns>
        public DataTable GetPlaylists()
        {
            DBManager dbm = DBManager.Instance;
            return dbm.executeQuery("Select title as Title from directories");
        }
        /// <summary>
        /// Adds songs given in the DataTable to DB
        /// </summary>
        /// <param name="songs">DataTable with songs info</param>
        public void AddSongs(DataTable songs)
        {
            string insertString = "";
            for (int i = 0; i < songs.Rows.Count; i++)
            {
                DataRow row = songs.Rows[i];
                string title = row["Title"].ToString();
                string artist = row["Artist"].ToString();
                string album = row["Album"].ToString();
                string genre = row["Genre"].ToString();
                string lenght = row["Length"].ToString();
                string path = row["Path"].ToString();
                string directoryid = row["DirectoryID"].ToString();
                insertString += "(\"" + title + "\",\"" + artist + "\",\"" + album + "\",\"" + genre + "\"," + lenght + ",\"" + path + "\"," + directoryid + ")";
                if (i != songs.Rows.Count - 1)
                    insertString += ",";
            }
            if (insertString.Equals(""))
                return;
            DBManager dbm = DBManager.Instance;
            dbm.executeNonQuery("Insert or replace into songs (title,artist,album,genre,length,path,id_directory) values " + insertString);
            Messenger.Default.Send<string, ApplicationViewModel>("ReloadLibrary");
        }
        /// <summary>
        /// Grabs scanned directories from DB. For each directory with LastWriteTime different than last seen scans for changes in this directory
        /// </summary>
        private void RescanLibrary()
        {
            DataTable dirs = GetDirectories();
            foreach(DataRow row in dirs.Rows)
            {
                string dirId = row["ID"].ToString();
                string path = row["Path"].ToString();
                long lastWriteTime = (long)row["LastWriteTime"];
                try
                {
                    if(!Directory.Exists(path))
                    {
                        DBManager dbm = DBManager.Instance;
                        dbm.executeNonQuery("DELETE FROM directories WHERE id=" + dirId);
                        dbm.executeNonQuery("DELETE FROM songs WHERE id_directory=" + dirId);
                    }else if (lastWriteTime < Directory.GetLastWriteTime(path).ToFileTime())
                    {
                        DBManager dbm = DBManager.Instance;
                        dbm.executeNonQuery("DELETE FROM songs WHERE id_directory=" + dirId);
                        DirectoryScanner ds = DirectoryScanner.Instance;
                        AddSongs(ds.Scan(path));
                    }
                }catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }
    }
}
