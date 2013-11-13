using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
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
    public class NowPlayingViewModel : ViewModelBase 
    {
        private ViewModelBase _currentViewModel;
        private List<ViewModelBase> _viewModels;
        private bool _albumArtSelected;
        private bool _biographySelected;
        private bool _lyricsSelected;
        public NowPlayingViewModel()
        {
            _viewModels = new List<ViewModelBase>();
            _viewModels.Add(new AlbumArtViewModel());
            _viewModels.Add(new BiographyViewModel());
            _viewModels.Add(new LyricsViewModel());

            MusicPlayer.Instance.BroadcastInfo();

            CurrentViewModel = _viewModels[0];
            AlbumArtSelected = true;
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
        public bool AlbumArtSelected
        {
            get
            {
                return _albumArtSelected;
            }
            set
            {
                _albumArtSelected = value;
                if (_albumArtSelected)
                {
                    BiographySelected = false;
                    LyricsSelected = false;
                    CurrentViewModel = _viewModels[0];
                }
                RaisePropertyChanged("AlbumArtSelected");
            }
        }
        public bool BiographySelected
        {
            get
            {
                return _biographySelected;
            }
            set
            {
                _biographySelected = value;
                if (_biographySelected)
                {
                    AlbumArtSelected = false;
                    LyricsSelected = false;
                    CurrentViewModel = _viewModels[1];
                }
                RaisePropertyChanged("BiographySelected");
            }
        }
        public bool LyricsSelected
        {
            get
            {
                return _lyricsSelected;
            }
            set
            {
                _lyricsSelected = value;
                if (_lyricsSelected)
                {
                    AlbumArtSelected = false;
                    BiographySelected = false;
                    CurrentViewModel = _viewModels[2];
                }
                RaisePropertyChanged("LyricsSelected");
            }
        }
    }
}