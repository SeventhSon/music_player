using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Music_Player.Messaging;
using Music_Player.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Music_Player.ViewModel
{
    public class GridViewModel:ViewModelBase
    {
        private List<SongModel> _songList;
        private RelayCommand<int> _playCommand;

        /// <summary>
        /// Class constructor, registering to receive messages
        /// </summary>
        public GridViewModel()
        {
            Messenger.Default.Register<List<SongModel>>
            (
                 this,
                 (action) => ReceiveMessage(action)
            );
            Messenger.Default.Register<NowPlayingPacket>
            (
                 this,
                 (action) => ReceiveMessage(action)
            );
            MusicPlayer.Instance.BroadcastSongs();
        }

        /// <summary>
        /// Receives the message with SongModel packet
        /// </summary>
        /// <param name="packet">Packet from SongModel list</param>
        private void ReceiveMessage(List<SongModel> packet)
        {
            SongList = packet;
        }

        /// <summary>
        /// Receives message with now playing packet and starts asynchronous task
        /// </summary>
        /// <param name="packet"></param>
        private void ReceiveMessage(NowPlayingPacket packet)
        {
            Task.Factory.StartNew(() =>
                {
                    foreach (SongModel sm in SongList)
                        if (packet.Path.Equals(sm.Path))
                            sm.NowPlaying = true;
                        else
                            sm.NowPlaying = false;
                });
        }
        /// <summary>
        /// Set the play queue for the audioplayer
        /// </summary>
        /// <param name="selectedIndex">Index of the song to play in the queue</param>
        private void UpdateQueue(int selectedIndex)
        {
            Task.Factory.StartNew(() =>
                {
                    foreach (SongModel sm in SongList)
                        sm.NowPlaying = false;
                    SongList[selectedIndex].NowPlaying = true;
                    MusicPlayer.Instance.setQueue(SongList, selectedIndex);
                });
        }

        /// <summary>
        /// Gets and sets SongList
        /// </summary>
        public List<SongModel> SongList
        {
            get
            {
                if (_songList == null)
                    _songList = new List<SongModel>();
                return _songList;
            }
            set
            {
                _songList = value;
                RaisePropertyChanged("SongList");
            }
        }
        public RelayCommand<int> PlayCommand
        {
            get
            {
                if (_playCommand == null)
                {
                    _playCommand = new RelayCommand<int>(selectedIndex => UpdateQueue(selectedIndex));
                }

                return _playCommand;
            }
        }
    }
}
