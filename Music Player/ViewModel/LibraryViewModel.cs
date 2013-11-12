using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Music_Player.Messaging;
using Music_Player.Model;
using System.Collections.Generic;

namespace Music_Player.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    /// 
    public class LibraryViewModel : ViewModelBase 
    {
        private ViewModelBase _currentViewModel;
        private List<ViewModelBase> _viewModels;
        private bool _songsSelected;
        private bool _genresSelected;
        private bool _albumsSelected;
        private bool _artistsSelected;
        public LibraryViewModel()
        {
            _viewModels = new List<ViewModelBase>();
            _viewModels.Add(new GridViewModel());
            _viewModels.Add(new TileViewModel());

            CurrentViewModel = _viewModels[0];
            SongsSelected = true;
        }
        public ViewModelBase CurrentViewModel
        {
            get
            {
                return _currentViewModel;
            }
            set
            {
                if (_currentViewModel == value)
                    return;
                _currentViewModel = value;
                RaisePropertyChanged("CurrentViewModel");
            }
        }
        public bool SongsSelected
        {
            get
            {
                return _songsSelected;
            }
            set
            {
                _songsSelected = value;
                if (_songsSelected)
                {
                    ArtistsSelected = false;
                    AlbumsSelected = false;
                    GenresSelected = false;
                    CurrentViewModel = _viewModels[0];
                    MusicPlayer.Instance.broadcastSongs();
                }
                RaisePropertyChanged("SongsSelected");
            }
        }
        public bool ArtistsSelected
        {
            get
            {
                return _artistsSelected;
            }
            set
            {
                _artistsSelected = value;
                if (_artistsSelected)
                {
                    SongsSelected = false;
                    AlbumsSelected = false;
                    GenresSelected = false;
                    CurrentViewModel = _viewModels[1];
                    MusicPlayer.Instance.broadcastArtists();
                }
                RaisePropertyChanged("ArtistsSelected");
            }
        }
        public bool AlbumsSelected
        {
            get
            {
                return _albumsSelected;
            }
            set
            {
                _albumsSelected = value;
                if (_albumsSelected)
                {
                    SongsSelected = false;
                    ArtistsSelected = false;
                    GenresSelected = false;
                    CurrentViewModel = _viewModels[1];
                    MusicPlayer.Instance.broadcastAlbums();
                }
                RaisePropertyChanged("AlbumsSelected");
            }
        }
        public bool GenresSelected
        {
            get
            {
                return _genresSelected;
            }
            set
            {
                _genresSelected = value;
                if (_genresSelected)
                {
                    SongsSelected = false;
                    AlbumsSelected = false;
                    ArtistsSelected = false;
                    CurrentViewModel = _viewModels[1];
                    MusicPlayer.Instance.broadcastGenres();
                }
                RaisePropertyChanged("GenresSelected");
            }
        }
    }
}