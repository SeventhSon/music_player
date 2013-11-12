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

        internal void forceBroadcastPlaylists()
        {
            //throw new NotImplementedException();
        }

        internal void forceBroadcastGenres()
        {
            //throw new NotImplementedException();
        }

        internal void forceBroadcastSongs()
        {
            //throw new NotImplementedException();
        }

        internal void forceBroadcastArtists()
        {
            //throw new NotImplementedException();
        }

        internal void forceBroadcastAlbums()
        {
            //throw new NotImplementedException();
        }
    }
}
