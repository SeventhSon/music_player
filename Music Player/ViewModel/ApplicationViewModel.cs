using System;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System.Windows.Threading;
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
    public class ApplicationViewModel : ViewModelBase 
    {
        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        /// 
        // Model
        private AudioPlayer musicPlayerModel;
        private LibraryManager libraryManagerModel;

        //ViewModel
        private string _nowPlayingTrack = "Play a song";
        private string _nowPlayingArtist = "";
        private int nowPlayingLenght = 0;
        private bool _isPlaying = false;
        private List<SongModel> _songList;
        private int _volume = 75;
        private int _percentagePlayed = 0;
        private int _timeEllapsed = 0;
        private DispatcherTimer timer;
        private RelayCommand<int> _playCommand;
        private RelayCommand _addCommand;
        private RelayCommand _nextCommand;
        private RelayCommand _prevCommand;

        //ViewModel navigation
        private List<ViewModelBase> _pageViewModels;
        private ViewModelBase _currentPageViewModel;
        private RelayCommand<ViewModelBase> _changePageCommand;

        public ApplicationViewModel()
        {
            //Register for string messages
            Messenger.Default.Register<string>(this, OnStringMessageReceived);

            //Initialize models
            musicPlayerModel = new AudioPlayer();
            libraryManagerModel = new LibraryManager();

            //Grab songs
            _songList = libraryManagerModel.GetSongs();
            
            timer = new DispatcherTimer();
            timer.Tick += dispatcherTimer_Tick;
            timer.Interval = new TimeSpan(0, 0, 1);
        }
        /// <summary>
        /// Timer counting the time since the start of the song till the end of it
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dispatcherTimer_Tick(object sender, System.EventArgs e)
        {
            if (TimeEllapsed >= nowPlayingLenght)
            {
                timer.Stop();
                return;
            }
            TimeEllapsed += 1;
        }
        /// <summary>
        /// Handler for message received event
        /// </summary>
        /// <param name="msg">Content of the string message</param>
        private void OnStringMessageReceived(string msg)
        {
            if (msg.Equals("ReloadLibrary"))
            {
                _songList = libraryManagerModel.GetSongs();
            }else if (msg.Equals("ReloadTrack"))
            {
                TimeEllapsed = 0;
                timer.Start();
                IsPlaying = true;
                NowPlayingArtist = musicPlayerModel.Artist;
                NowPlayingTrack = musicPlayerModel.Track + " - " + musicPlayerModel.Album;
                nowPlayingLenght = musicPlayerModel.GetTrackLength();
            }
        }
        /// <summary>
        /// Set the play queue for the audioplayer
        /// </summary>
        /// <param name="selectedIndex">Index of the song to play in the queue</param>
        private void UpdateQueue(int selectedIndex)
        {
            musicPlayerModel.SetQueue(SongList,selectedIndex);
        }
        /// <summary>
        /// Opens directory browser dialog and scans recursively selected directory
        /// </summary>
        private void ScanFolder()
        {
            var dialog = new System.Windows.Forms.FolderBrowserDialog();
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                DirectoryScanner ds = DirectoryScanner.Instance;
                libraryManagerModel.AddSongs(ds.ScanRecursive(dialog.SelectedPath,null));
            }
        }
        /// <summary>
        /// Play next song from the play queue
        /// </summary>
        private void NextSong()
        {
            musicPlayerModel.Next();
        }
        /// <summary>
        /// Play previous song from the play queue
        /// </summary>
        private void PrevSong()
        {
            musicPlayerModel.Prev();
        }
        private void ChangeViewModel(ViewModelBase p)
        {

        }
        //Bound properties and commands
        public RelayCommand<ViewModelBase> ChangePageCommand
        {
            get
            {
                if (_changePageCommand == null)
                {
                    _changePageCommand = new RelayCommand<ViewModelBase>(page => ChangeViewModel(page));
                }

                return _changePageCommand;
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
        public RelayCommand AddCommand
        {
            get
            {
                if (_addCommand == null)
                {
                    _addCommand = new RelayCommand(ScanFolder);
                }

                return _addCommand;
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
        public List<ViewModelBase> PageViewModels
        {
            get
            {
                if (_pageViewModels == null)
                    _pageViewModels = new List<ViewModelBase>();

                return _pageViewModels;
            }
        }
        public List<SongModel> SongList
        {
            get;
            set
            {
                _songList = value;
                RaisePropertyChanged("SongList");
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
                if (_nowPlayingTrack == value)
                    return;
                _nowPlayingTrack = value;
                RaisePropertyChanged("NowPlayingTrack");
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
                if (_nowPlayingArtist == value)
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
                    timer.Start();
                    musicPlayerModel.Play();
                }
                else
                {
                    timer.Stop();
                    musicPlayerModel.Pause();
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
                musicPlayerModel.ChangeVolume(_volume);
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
                    musicPlayerModel.Seek(TimeEllapsed);
                }
            }
        }
    }
}