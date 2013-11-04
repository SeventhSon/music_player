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
            Messenger.Default.Send<string, MainViewModel>("ReloadLibrary");
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
            foreach(DataRow row in songs.Rows)
            { 
                string title = row["Title"].ToString();
                string artist = row["Artist"].ToString();
                string album = row["Album"].ToString();
                string genre = row["Genre"].ToString();
                string lenght = row["Lenght"].ToString();
                string directoryid = row["DirectoryID"].ToString();
                insertString += "(" + title + "," + artist + "," + album + "," + genre + "," + lenght + "," + directoryid + ")";
            }
            DBManager dbm = DBManager.Instance;
            dbm.executeNonQuery("Insert or ignore into songs (title,artist,album,genre,length,directoryid) values " + insertString);
        }
        private int RescanLibrary()
        {
            int scannedItems = 0;
            DataTable dirs = GetDirectories();
            foreach(DataRow row in dirs.Rows)
            {
                string dirId = row["ID"].ToString();
                string path = row["Path"].ToString();
                long lastWriteTime = (long)row["last_write_time"];
                try
                {
                    if(lastWriteTime == File.GetLastWriteTime(path).ToFileTime())
                    {
                        DirectoryScanner ds = DirectoryScanner.Instance;
                        AddSongs(ds.ScanRecursive(path));
                    }
                }catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            return scannedItems; 
        }
    }
}
