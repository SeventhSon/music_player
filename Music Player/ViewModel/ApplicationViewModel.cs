using System;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System.Windows.Threading;
using Music_Player.Model;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Music_Player.Messaging;

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
    public class ApplicationViewModel : ViewModelBase 
    {
        // Model
        private MusicPlayer mp;

        //ViewModel
        private string _nowPlayingTrack = "Select a track";
        private string _nowPlayingArtist = "To start playing";
        private string _nowPlayingAlbum = "";
        private int nowPlayingLenght = 0;
        private bool _isPlaying = false;
        private List<PlaylistModel> _playlistList;
        private int _volume = 75;
        private int _percentagePlayed = 0;
        private int _timeEllapsed = 0;
        private DispatcherTimer progressTimer;
        private RelayCommand _nextCommand;
        private RelayCommand _prevCommand;

        //ViewModel navigation
        private List<NavigationItemModel> _navigation;
        private ViewModelBase _currentPageViewModel;
        private RelayCommand<NavigationItemModel> _navigateCommand;
        private int _selectedNavigation;
        public ApplicationViewModel()
        {
            //Adding navigation items and their associated viewModels
            Navigation.Add(new NavigationItemModel(new NowPlayingViewModel(),"Now Playing"));
            Navigation.Add(new NavigationItemModel(new LibraryViewModel(),"Library"));

            //Set the starting page to LibraryView and select it
            ChangeViewModel(Navigation[1]);
            SelectedNavigation = 1;

            //Register for NowPlaying packets
            Messenger.Default.Register<NowPlayingPacket>
            (
                 this,
                 (action) => ReceiveMessage(action)
            );
            //Register for Playlist packets
            Messenger.Default.Register<List<PlaylistModel>>
            (
                 this,
                 (action) => ReceiveMessage(action)
            );

            //Initialize model
            mp = MusicPlayer.Instance;
            mp.BroadcastPlaylists();
            
            //Setup the progressTimer for updating the slider
            progressTimer = new DispatcherTimer();
            progressTimer.Tick += advanceTimeEllapsed;
            progressTimer.Interval = new TimeSpan(0, 0, 1);
        }
        #region functions
        /// <summary>
        /// Handle NowPlayingPacket. Fill own properties with received data
        /// </summary>
        /// <param name="packet">Object containting every kind of information about the song</param>
        private void ReceiveMessage(NowPlayingPacket packet)
        {
            nowPlayingLenght = packet.Length;
            NowPlayingTrack = packet.Title.Equals("") ? packet.Path.Split('\\').Last(): packet.Title;
            NowPlayingArtist = packet.Artist;
            NowPlayingAlbum = packet.Album;
            TimeEllapsed = 0;
            IsPlaying = true;
            if (!progressTimer.IsEnabled)
                progressTimer.Start();
        }
        private void ReceiveMessage(List<PlaylistModel> packet)
        {
            PlaylistList = packet;
        }
        /// <summary>
        /// Timer counting the time since the start of the song till the end of it
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void advanceTimeEllapsed(object sender, System.EventArgs e)
        {
            if (TimeEllapsed >= nowPlayingLenght)
            {
                progressTimer.Stop();
                return;
            }
            TimeEllapsed += 1;
        }

        /// <summary>
        /// Play next song from the play queue
        /// </summary>
        private void NextSong()
        {
            mp.NextSong();
        }
        /// <summary>
        /// Play previous song from the play queue
        /// </summary>
        private void PrevSong()
        {
            mp.PrevSong();
        }
        private void ChangeViewModel(NavigationItemModel nav)
        {
            if (!Navigation.Contains(nav))
                Navigation.Add(nav);

            CurrentPageViewModel = Navigation.FirstOrDefault(vm => vm == nav).ViewModel;
        }
        #endregion
        //Bound properties and commands
        #region commands
        public RelayCommand<NavigationItemModel> NavigateCommand
        {
            get
            {
                if (_navigateCommand == null)
                {
                    _navigateCommand = new RelayCommand<NavigationItemModel>(nav => ChangeViewModel(nav));
                }

                return _navigateCommand;
            }
        }
        public RelayCommand NextCommand
        {
            get
            {
                if (_nextCommand == null)
                {
                    _nextCommand = new RelayCommand(NextSong);
                }

                return _nextCommand;
            }
        }
        public RelayCommand PrevCommand
        {
            get
            {
                if (_prevCommand == null)
                {
                    _prevCommand = new RelayCommand(PrevSong);
                }

                return _prevCommand;
            }
        }
        #endregion
        #region properties
        public ViewModelBase CurrentPageViewModel
        {
            get
            {
                return _currentPageViewModel;
            }
            set
            {
                if (_currentPageViewModel != value)
                {
                    _currentPageViewModel = value;
                    RaisePropertyChanged("CurrentPageViewModel");
                }
            }
        }
        public List<NavigationItemModel> Navigation
        {
            get
            {
                if (_navigation == null)
                    _navigation = new List<NavigationItemModel>();

                return _navigation;
            }
        }
        public List<PlaylistModel> PlaylistList
        {
            get
            {
                if (_playlistList == null)
                    _playlistList = new List<PlaylistModel>();
                return _playlistList;
            }
            set
            {
                _playlistList = value;
                RaisePropertyChanged("PlaylistList");
            }
        }
        public int SelectedNavigation
        {
            get
            {
                return _selectedNavigation;
            }
            set
            {
                if (_selectedNavigation == value)
                    return;
                _selectedNavigation = value;
                ChangeViewModel(Navigation[SelectedNavigation]);
                RaisePropertyChanged("SelectedNavigation");
            }
        }
        public string NowPlayingTrack
        {
            get 
            {
                return _nowPlayingTrack.Equals("")?"Unknown Track":_nowPlayingTrack;
            }
            set 
            {
                if (_nowPlayingTrack.Equals(value))
                    return;
                _nowPlayingTrack = value;
                RaisePropertyChanged("NowPlayingTrack");
            }
        }
        public string NowPlayingAlbum
        {
            get
            {
                return _nowPlayingAlbum.Equals("") ? "Unknown Album" : _nowPlayingAlbum;
            }
            set
            {
                if (_nowPlayingAlbum.Equals(value))
                    return;
                _nowPlayingAlbum = value;
                RaisePropertyChanged("NowPlayingAlbum");
            }
        }
        public string NowPlayingArtist
        {
            get
            {
                return _nowPlayingArtist.Equals("") ? "Unknown Artist" : _nowPlayingArtist; ;
            }
            set
            {
                if (_nowPlayingArtist.Equals(value))
                    return;
                _nowPlayingArtist = value;
                RaisePropertyChanged("NowPlayingArtist");
            }
        }
        public bool IsPlaying
        {
            get
            {
                return _isPlaying;
            }
            set
            {
                if (_isPlaying == value)
                    return;
                _isPlaying = value;
                RaisePropertyChanged("IsPlaying");
                if (_isPlaying)
                {
                    if(!progressTimer.IsEnabled)
                        progressTimer.Start();
                    mp.PlaySong();
                }
                else
                {
                    if(progressTimer.IsEnabled)
                        progressTimer.Stop();
                    mp.PauseSong();
                }
            }
        }
        public int Volume
        {
            get
            {
                return _volume;
            }
            set
            {
                if (_volume == value)
                    return;
                _volume = value;
                mp.ChangeSongVolume(_volume);
                RaisePropertyChanged("Volume");
            }
        }
        public int TimeEllapsed
        {
            get
            {
                return _timeEllapsed;
            }
            set
            {
                if (_timeEllapsed == value)
                    return;
                _timeEllapsed = value;
                PercentagePlayed = (int)((double)(_timeEllapsed) / (double)(nowPlayingLenght) * 1000);
                RaisePropertyChanged("TimeEllapsed");
            }
        }
        public int PercentagePlayed
        {
            get
            {
                return _percentagePlayed;
            }
            set
            {
                if (_percentagePlayed == value)
                    return;
                RaisePropertyChanging("PercentagePlayed");
                int oldvalue = _percentagePlayed;
                _percentagePlayed = value;
                RaisePropertyChanged("PercentagePlayed");
                //Detect if change made by user or if slider just naturally progressed
                if ((int)((double)TimeEllapsed / (double)nowPlayingLenght * 1000) != _percentagePlayed)
                {
                    //If so change timeEllapsed to new value from slider position and seek
                    TimeEllapsed = (int)((double)_percentagePlayed/1000 * nowPlayingLenght);
                    mp.SeekSong(TimeEllapsed);
                }
            }
        }
        #endregion
    }
}