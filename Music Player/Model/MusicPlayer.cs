using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Music_Player.Model
{
    public class MusicPlayer
    {
        private static volatile MusicPlayer _instance;
        private static object monitor = new Object();
        private LibraryManager libraryManager;
        private DirectoryScanner directoryScanner;
        private AudioPlayer audioPlayer;
        private InfoScrapper infoScrapper;

        /// <summary>
        /// MusicPlayer class constructor, creating libraryManager, directoryScanner, audioPlayer and infoScrapper
        /// </summary>
        private MusicPlayer()
        {
            libraryManager = new LibraryManager();
            directoryScanner = new DirectoryScanner();
            audioPlayer = new AudioPlayer();
            infoScrapper = new InfoScrapper();
        }

        /// <summary>
        /// Holds the handle to singleton instance
        /// </summary>
        public static MusicPlayer Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (monitor)
                    {
                        if (_instance == null)
                            _instance = new MusicPlayer();
                    }
                }
                return _instance;
            }
        }

        /// <summary>
        /// Asynchronous task calling ForceNowPlayingBroadcast
        /// </summary>
        public void BroadcastNowPlaying()
        {
            Task.Factory.StartNew(() =>
                {
                    audioPlayer.ForceNowPlayingBroadcast();
                });
        }

        /// <summary>
        /// Asynchronous task calling SetQueue
        /// </summary>
        public void setQueue(List<SongModel> SongList, int selectedIndex)
        {
            Task.Factory.StartNew(() =>
                {
                    audioPlayer.SetQueue(SongList, selectedIndex);
                });
        }

        /// <summary>
        /// Asynchronous task calling Next
        /// </summary>
        public void NextSong()
        {
            Task.Factory.StartNew(() =>
                {
                    audioPlayer.Next();
                });
        }

        /// <summary>
        /// Asynchronous task calling Prev
        /// </summary>
        public void PrevSong()
        {
            Task.Factory.StartNew(() =>
                {
                    audioPlayer.Prev();
                });
        }

        /// <summary>
        /// Asynchronous task calling Play
        /// </summary>
        public void PlaySong()
        {
            Task.Factory.StartNew(() =>
                {
                    audioPlayer.Play();
                });
        }

        /// <summary>
        /// Asynchronous task calling Pause
        /// </summary>
        public void PauseSong()
        {
            Task.Factory.StartNew(() =>
                {
                    audioPlayer.Pause();
                });
        }

        /// <summary>
        /// Asynchronous task calling ChangeVolume 
        /// </summary>
        public void ChangeSongVolume(int volume)
        {
            Task.Factory.StartNew(() =>
                {
                    audioPlayer.ChangeVolume(volume);
                });
        }

        /// <summary>
        /// Calling Seek function
        /// </summary>
        public void SeekSong(int timeEllapsed)
        {
            audioPlayer.Seek(timeEllapsed);
        }

        /// <summary>
        /// Asynchronous task calling ForceBroadcastPlaylists 
        /// </summary>
        public void BroadcastPlaylists()
        {
            Task.Factory.StartNew(() =>
                {
                    libraryManager.ForceBroadcastPlaylists();
                });
        }

        /// <summary>
        /// Asynchronous task calling ForceBroadcastInfo
        /// </summary>
        public void BroadcastInfo()
        {
            Task.Factory.StartNew(() =>
                {
                    infoScrapper.ForceBroadcastInfo();
                });
        }

        /// <summary>
        /// Asynchronous task scanning directories, broadcasting directories and songs packet
        /// </summary>
        /// <param name="p">Path of directory to scan</param>
        public void ScanDirectory(string p)
        {
            Task.Factory.StartNew(() =>
                {
                    directoryScanner.ScanRecursive(p);
                    directoryScanner.ForceBroadcastDirectories();
                    libraryManager.ForceBroadcastSongs();
                });

        }

        /// <summary>
        /// Asynchronous task calling ForceBroadcastGenres 
        /// </summary>
        public void BroadcastGenres()
        {
            Task.Factory.StartNew(() =>
            {
                libraryManager.ForceBroadcastGenres();
            });
        }

        /// <summary>
        /// Asynchronous task calling ForceBroadcastAlbums 
        /// </summary>
        public void BroadcastAlbums()
        {
            Task.Factory.StartNew(() =>
                {
                    libraryManager.ForceBroadcastAlbums();
                });
        }

        /// <summary>
        /// Asynchronous task calling ForceBroadcastArtists 
        /// </summary>
        public void BroadcastArtists()
        {
            Task.Factory.StartNew(() =>
                {
                    libraryManager.ForceBroadcastArtists();
                });
        }

        /// <summary>
        /// Asynchronous task calling ForceBroadcastSongs 
        /// </summary>
        public void BroadcastSongs()
        {
            Task.Factory.StartNew(() =>
                {
                    libraryManager.ForceBroadcastSongs();
                });
        }

        /// <summary>
        /// Asynchronous task calling ForceBroadcastDirectories 
        /// </summary>
        public void BroadcastDirectories()
        {
            Task.Factory.StartNew(() =>
                {
                    directoryScanner.ForceBroadcastDirectories();
                });
        }
    }
}
