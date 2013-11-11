using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
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
        private RelayCommand<string> _sortCommand;
        /// <summary>
        /// Set the play queue for the audioplayer
        /// </summary>
        /// <param name="selectedIndex">Index of the song to play in the queue</param>
        private void UpdateQueue(int selectedIndex)
        {
            MusicPlayer.Instance.setQueue(SongList, selectedIndex);
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
    }
}
