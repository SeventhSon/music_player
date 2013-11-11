using GalaSoft.MvvmLight;
using Music_Player.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Music_Player.ViewModel
{
    public class TileViewModel:ViewModelBase
    {
        private List<PhotoModel> _tiles;
        public List<PhotoModel> TileData
        { 
            get { return _tiles; }
            set 
            {
                _tiles = value;
                RaisePropertyChanged("TileData");
            }
        }
    }
}
