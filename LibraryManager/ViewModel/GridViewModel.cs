using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Music_Player.Messaging;
using LibraryManager.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryServiceLib;

namespace LibraryManager.ViewModel
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
            //Request songs
        }
        private void ReceiveMessage(List<SongModel> packet)
        {
            SongList = packet;
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
    }
}
