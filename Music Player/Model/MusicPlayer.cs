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
        /// Asynchronous
        /// </summary>
        public void BroadcastNowPlaying()
        {
            Task.Factory.StartNew(() =>
                {
                    audioPlayer.ForceNowPlayingBroadcast();
                });
        }

        public void setQueue(List<SongModel> SongList, int selectedIndex)
        {
            Task.Factory.StartNew(() =>
                {
                    audioPlayer.SetQueue(SongList, selectedIndex);
                });
        }

        public void NextSong()
        {
            Task.Factory.StartNew(() =>
                {
                    audioPlayer.Next();
                });
        }

        public void PrevSong()
        {
            Task.Factory.StartNew(() =>
                {
                    audioPlayer.Prev();
                });
        }

        public void PlaySong()
        {
            Task.Factory.StartNew(() =>
                {
                    audioPlayer.Play();
                });
        }

        public void PauseSong()
        {
            Task.Factory.StartNew(() =>
                {
                    audioPlayer.Pause();
                });
        }

        public void ChangeSongVolume(int volume)
        {
            Task.Factory.StartNew(() =>
                {
                    audioPlayer.ChangeVolume(volume);
                });
        }

        public void SeekSong(int timeEllapsed)
        {
            audioPlayer.Seek(timeEllapsed);
        }

        public void BroadcastPlaylists()
        {
            Task.Factory.StartNew(() =>
                {
                    libraryManager.ForceBroadcastPlaylists();
                });
        }

        public void BroadcastInfo()
        {
            Task.Factory.StartNew(() =>
                {
                    infoScrapper.ForceBroadcastInfo();
                });
        }

        public void ScanDirectory(string p)
        {
            Task.Factory.StartNew(() =>
                {
                    directoryScanner.ScanRecursive(p);
                    directoryScanner.ForceBroadcastDirectories();
                    libraryManager.ForceBroadcastSongs();
                });

        }

        public void BroadcastGenres()
        {
            Task.Factory.StartNew(() =>
            {
                libraryManager.ForceBroadcastGenres();
            });
        }

        public void BroadcastAlbums()
        {
            Task.Factory.StartNew(() =>
                {
                    libraryManager.ForceBroadcastAlbums();
                });
        }

        public void BroadcastArtists()
        {
            Task.Factory.StartNew(() =>
                {
                    libraryManager.ForceBroadcastArtists();
                });
        }

        public void BroadcastSongs()
        {
            Task.Factory.StartNew(() =>
                {
                    libraryManager.ForceBroadcastSongs();
                });
        }

        public void BroadcastDirectories()
        {
            Task.Factory.StartNew(() =>
                {
                    directoryScanner.ForceBroadcastDirectories();
                });
        }
    }
}
