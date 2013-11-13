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
        private RelayCommand<SongModel> _saveCommand;
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
        private void ReceiveMessage(List<SongModel> packet)
        {
            SongList = packet;
        }
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
        public RelayCommand<SongModel> SaveCommand
        {
            get
            {
                if (_saveCommand == null)
                {
                    _saveCommand = new RelayCommand<SongModel>(selectedItem => SaveSongs(selectedItem));
                }

                return _saveCommand;
            }
        }

        private void SaveSongs(SongModel selectedItem)
        {
            MusicPlayer.Instance.SaveSong(selectedItem);
        }
    }
}
