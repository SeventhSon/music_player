using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Data;
using System.Windows;
using GalaSoft.MvvmLight.Messaging;
using System.Windows.Threading;
using System;

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
    public class MainViewModel : ViewModelBase 
    {
        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        /// 
        // Model
        private MusicPlayer musicPlayerModel;
        private LibraryManager libraryManagerModel;

        //ViewModel
        private string nowPlayingTrack = "Play a song";
        private string nowPlayingArtist = "";
        private int nowPlayingLenght = 0;
        private int nowPlayingIndex = -1;
        private bool isPlaying = false;
        private DataView results = new DataView();
        private DataTable songs = new DataTable();
        private int volume = 100;
        private int percentagePlayed = 0;
        private int timeEllapsed = 0;
        private DispatcherTimer timer;
        public MainViewModel()
        {
            PlayCommand = new RelayCommand(() => UpdateNowPlaying());
            Messenger.Default.Register<string>(this, OnStringMessageReceived);

            musicPlayerModel = new MusicPlayer();
            libraryManagerModel = new LibraryManager();

            Results = libraryManagerModel.GetSongs().AsDataView();
            timer = new DispatcherTimer();
            timer.Tick += dispatcherTimer_Tick;
            timer.Interval = new TimeSpan(0, 0, 1);
        }

        private void dispatcherTimer_Tick(object sender, System.EventArgs e)
        {
            TimeEllapsed += 1;
            PercentagePlayed = TimeEllapsed / nowPlayingLenght;
        }
        private void OnStringMessageReceived(string msg)
        {
            if (msg.Equals("ReloadLibrary"))
            {
                songs = libraryManagerModel.GetSongs();
                Results = songs.AsDataView();
            }else if (msg.Equals("ChangedTrack"))
            {
                NowPlayingIndex = musicPlayerModel.Index;
            }
        }

        private void UpdateNowPlaying(int selectedIndex)
        {
            NowPlayingIndex = selectedIndex;
            musicPlayerModel.Index = NowPlayingIndex;
            musicPlayerModel.Queue = songs;
        }
        public RelayCommand PlayCommand
        {
            get;
            private set;
        }
        public RelayCommand AddFolder
        {
            get;
            private set;
        }
        public int NowPlayingIndex
        {
            get
            {
                return nowPlayingIndex;
            }
            set
            {
                if (nowPlayingIndex == value)
                    return;
                nowPlayingIndex = value;
                RaisePropertyChanged("NowPlayingIndex");
                DataTable Queue = musicPlayerModel.Queue;
                NowPlayingArtist = Queue.Rows[NowPlayingIndex]["Artist"].ToString();
                NowPlayingTrack = Queue.Rows[NowPlayingIndex]["Title"].ToString();
                nowPlayingLenght = (int)Queue.Rows[NowPlayingIndex]["Time"];
            }
        }
        public string NowPlayingTrack
        {
            get 
            {
                return nowPlayingTrack;
            }
            set 
            {
                if (nowPlayingTrack == value)
                    return;
                nowPlayingTrack = value;
                RaisePropertyChanged("NowPlayingTrack");
            }
        }
        public string NowPlayingArtist
        {
            get
            {
                return nowPlayingArtist;
            }
            set
            {
                if (nowPlayingArtist == value)
                    return;
                nowPlayingArtist = value;
                RaisePropertyChanged("NowPlayingArtist");
            }
        }
        public bool IsPlaying
        {
            get
            {
                return isPlaying;
            }
            set
            {
                if (isPlaying == value)
                    return;
                isPlaying = value;
                RaisePropertyChanged("IsPlaying");
                timer.Start();
                if (isPlaying)
                    musicPlayerModel.Play();
                else
                    musicPlayerModel.Pause();
            }
        }
        public DataView Results
        {
            get
            {
                return results;
            }
            set
            {
                if (results == value)
                    return;
                results = value;
                RaisePropertyChanged("Results");
            }
        }
        public int Volume
        {
            get
            {
                return volume;
            }
            set
            {
                if (volume == value)
                    return;
                volume = value;
                musicPlayerModel.changeVolume(volume);
                RaisePropertyChanged("Volume");
            }
        }
        public int TimeEllapsed
        {
            get
            {
                return timeEllapsed;
            }
            set
            {
                if (timeEllapsed == value)
                    return;
                timeEllapsed = value;
                RaisePropertyChanged("TimeEllapsed");
                //PercentagePlayed = timeEllapsed / nowPlayingLenght;
                
            }
        }
        public int PercentagePlayed
        {
            get
            {
                return percentagePlayed;
            }
            set
            {
                if (percentagePlayed == value)
                    return;
                percentagePlayed = value;
                RaisePropertyChanged("PercentagePlayed");
                TimeEllapsed = percentagePlayed * nowPlayingLenght/100;
                
            }
        }
    }
}