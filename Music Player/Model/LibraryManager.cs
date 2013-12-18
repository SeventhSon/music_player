using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using GalaSoft.MvvmLight.Messaging;
using Music_Player.ViewModel;
using System.IO;
using Music_Player.Messaging;
using System.Threading;
using System.Windows;
using Music_Player.LibraryServiceReference;

namespace Music_Player.Model
{
    class LibraryManager
    {
        /// <summary>
        /// Create new object of Library Manager and rescans every already scanned directory
        /// </summary>
        /// 
        private object filelock = new Object();
        private LibraryServiceClient lsc;
        private string nowPlayingPath ="";
        public LibraryManager()
        {
            lsc = new LibraryServiceClient();
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
            GalaSoft.MvvmLight.Messaging.Messenger.Default.Send<List<SongModel>>(lsc.GetSongs());    
        }

        public void ForceBroadcastArtists()
        {
            //throw new NotImplementedException();
        }

        public void ForceBroadcastAlbums()
        {
            //throw new NotImplementedException();
        }
        private void ReceiveMessage(NowPlayingPacket packet)
        {
            nowPlayingPath = packet.Path;
        }
    }
}
