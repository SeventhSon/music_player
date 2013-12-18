using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.IO;
using System.Data;

namespace LibraryServiceLib
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in both code and config file together.
    [ServiceBehavior(IncludeExceptionDetailInFaults = true)]
    public class LibraryService : ILibraryService
    {
        public string dummy()
        {
            return "Hello\n";
        }
        public List<SongModel> GetSongs()
        {
            List<SongModel> packet=new List<SongModel>();
            foreach (DataRow row in DBManager.Instance.ExecuteQuery("Select * from songs order by artist,album").Rows)
            {
                SongModel song = new SongModel(row);
                packet.Add(song);
            }
            return packet;
        }

        public List<SongModel> GetSongsByGenre(string genre)
        {
            List<SongModel> packet = new List<SongModel>();
            foreach (DataRow row in DBManager.Instance.ExecuteQuery("Select * from songs where genre='" + genre + "' order by artist,album").Rows)
            {
                SongModel song = new SongModel(row);
                packet.Add(song);
            }
            return packet;
        }

        public List<SongModel> GetSongsByAlbum(string album)
        {
            List<SongModel> packet = new List<SongModel>();
            foreach (DataRow row in DBManager.Instance.ExecuteQuery("Select * from songs where album='" + album + "' order by artist,genre").Rows)
            {
                SongModel song = new SongModel(row);
                packet.Add(song);
            }
            return packet;
        }

        public Stream GetSongStream(string path, float offsetPercentage)
        {
            MemoryStream ms = new MemoryStream();
            if (!path.Split('.').Last().Equals("mp3"))
                return ms;
            using (var file = File.OpenRead(path))
            {
                file.Position = (long) (offsetPercentage * file.Length);
                file.CopyTo(ms);
            }
            ms.Position = 0L;
            return ms;
        }

        public Stream GetAlbumArtStream(string path)
        {
            MemoryStream ms = new MemoryStream();
            using (var file = File.OpenRead(path))
            {
                file.CopyTo(ms);
            }
            ms.Position = 0L;
            return ms;
        }
    }
}
