using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Data;
using System.Windows;
using GalaSoft.MvvmLight.Messaging;

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
        public MainViewModel()
        {
            PlayCommand = new RelayCommand(() => UpdateNowPlaying());
            Messenger.Default.Register<string>(this, OnStringMessageReceived);

            musicPlayerModel = new MusicPlayer();
            libraryManagerModel = new LibraryManager();

            Results = libraryManagerModel.GetSongs().AsDataView();
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

                NowPlayingArtist = musicPlayerModel.Artist;
                NowPlayingTrack = row["Title"].ToString();
                nowPlayingLenght = (int)row["Time"];
            }
        }

        private void UpdateNowPlaying()
        {
            DataRow row = songs.Rows[NowPlayingIndex];
            NowPlayingArtist = row["Artist"].ToString();
            NowPlayingTrack = row["Title"].ToString();
            nowPlayingLenght = (int)row["Time"];
        }
        public RelayCommand PlayCommand
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