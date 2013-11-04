using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using GalaSoft.MvvmLight.Messaging;
using Music_Player.ViewModel;
using System.IO;

namespace Music_Player
{
    class LibraryManager
    {
        public LibraryManager()
        {
            RescanLibrary();
        }
        public DataTable GetSongs()
        {
            DBManager dbm = DBManager.Instance;
            return dbm.executeQuery("Select title as Title, artist as Artist, album as Album, genre as Genre, length as Time,path as Path from songs");
        }
        public DataTable GetDirectories()
        {
            DBManager dbm = DBManager.Instance;
            return dbm.executeQuery("Select id as ID, path as Path, last_write_time as LastWriteTime from directories");
        }

        public DataTable GetPlaylists()
        {
            DBManager dbm = DBManager.Instance;
            return dbm.executeQuery("Select title as Title from directories");
        }
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
            DBManager dbm = DBManager.Instance;
            dbm.executeNonQuery("Insert or ignore into songs (title,artist,album,genre,length,path,id_directory) values " + insertString);
            Messenger.Default.Send<string, MainViewModel>("ReloadLibrary");
        }
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
                    if(lastWriteTime < File.GetLastWriteTime(path).ToFileTime())
                    {
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
