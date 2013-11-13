using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using Music_Player.Messaging;
using Music_Player.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Music_Player.ViewModel
{
    public class AlbumArtViewModel: ViewModelBase
    {
        private BitmapImage _albumArtImg;
        private BitmapImage defaultImg;

        /// <summary>
        /// Class constructor, registering to receive message, setting default album art image
        /// </summary>
        public AlbumArtViewModel()
        {
            Messenger.Default.Register<NowPlayingPacket>
            (
                 this,
                 (action) => ReceiveMessage(action)
            );
            defaultImg = new BitmapImage(new Uri("/Images/albumart.jpg", UriKind.Relative));
            AlbumArtImg = defaultImg;
            MusicPlayer.Instance.BroadcastNowPlaying();
        }

        /// <summary>
        /// Gets and sets image to be displayed
        /// </summary>
        public BitmapImage AlbumArtImg
        {
            get { return _albumArtImg; }
            set 
            {
                _albumArtImg = value;
                RaisePropertyChanged("AlbumArtImg");
            }
        }

        /// <summary>
        /// Receives message
        /// </summary>
        /// <param name="packet"></param>
        private void ReceiveMessage(NowPlayingPacket packet)
        {
            AlbumArtImg = packet.AlbumArt;
        }
    }
}
