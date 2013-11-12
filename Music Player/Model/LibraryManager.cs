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
        /// 
        private DBManager dbm;
        public LibraryManager()
        {
            dbm = DBManager.Instance;
        }

        public void ForceBroadcastPlaylists()
        {
            //throw new NotImplementedException();
        }

        public void ForceBroadcastGenres()
        {
            //throw new NotImplementedException();
        }

        public void ForceBroadcastSongs()
        {
            DataTable dt = dbm.executeQuery("Select * from songs");
            List<SongModel> packet = new List<SongModel>();
            foreach (DataRow row in dt.Rows)
                packet.Add(new SongModel(row));
            GalaSoft.MvvmLight.Messaging.Messenger.Default.Send<List<SongModel>>(packet);    
        }

        public void ForceBroadcastArtists()
        {
            //throw new NotImplementedException();
        }

        public void ForceBroadcastAlbums()
        {
            //throw new NotImplementedException();
        }
        public void SaveSongData(SongModel sm)
        {

        }
    }
}
