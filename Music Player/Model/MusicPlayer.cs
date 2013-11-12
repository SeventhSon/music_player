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
        private MusicPlayer()
        {
            libraryManager = new LibraryManager();
            directoryScanner = new DirectoryScanner();
            audioPlayer = new AudioPlayer();
            infoScrapper = new InfoScrapper();
        }
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
        public void broadcastNowPlaying()
        {
            Task.Factory.StartNew(() =>
                {
                    audioPlayer.ForceNowPlayingBroadcast();
                });
        }

        public void setQueue(List<SongModel> SongList, int selectedIndex)
        {
            audioPlayer.SetQueue(SongList,selectedIndex);
        }

        public void NextSong()
        {
            audioPlayer.Next();
        }

        public void PrevSong()
        {
            audioPlayer.Prev();
        }

        public void PlaySong()
        {
            audioPlayer.Play();
        }

        public void PauseSong()
        {
            audioPlayer.Pause();
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

        public void broadcastPlaylists()
        {
            Task.Factory.StartNew(() =>
                {
                    libraryManager.ForceBroadcastPlaylists();
                });
        }

        public void broadcastInfo()
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

        public void broadcastGenres()
        {
            Task.Factory.StartNew(() =>
            {
                libraryManager.ForceBroadcastGenres();
            });
        }

        public void broadcastAlbums()
        {
            Task.Factory.StartNew(() =>
                {
                    libraryManager.ForceBroadcastAlbums();
                });
        }

        public void broadcastArtists()
        {
            Task.Factory.StartNew(() =>
                {
                    libraryManager.ForceBroadcastArtists();
                });
        }

        public void broadcastSongs()
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
