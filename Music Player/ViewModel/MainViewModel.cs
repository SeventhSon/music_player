using GalaSoft.MvvmLight;
using System.Data;
using System.Windows;
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
    public class MainViewModel : ViewModelBase 
    {
        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        /// 
        private string nowPlayingTrack = "Select a track to play";
        private string nowPlayingArtist = "Not playing";
        private bool isPlaying = true;
        private DataView results = new DataView();
        private int volume = 100;
        public MainViewModel()
        {
            ////if (IsInDesignMode)
            ////{
            ////    // Code runs in Blend --> create design time data.
            ////}
            ////else
            ////{
            ////    // Code runs "for real"
            ////}
        }
        public string NowPlayingTrack
        {
            get 
            {
                return nowPlayingTrack;
            }
            set 
            {
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
                volume = value;
                RaisePropertyChanged("Volume");
            }
        }
    }
}