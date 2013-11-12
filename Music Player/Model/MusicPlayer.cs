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
            audioPlayer.forceNowPlayingBroadcast();
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
            audioPlayer.ChangeVolume(volume);
        }

        public void SeekSong(int timeEllapsed)
        {
            audioPlayer.Seek(timeEllapsed);
        }

        public void broadcastPlaylists()
        {
            libraryManager.forceBroadcastPlaylists();
        }

        public void broadcastInfo()
        {
            infoScrapper.forceBroadcastInfo();
        }

        public void ScanDirectory(string p)
        {
            directoryScanner.ScanRecursive(p);
            directoryScanner.ForceBroadcastDirectories();
        }

        public void broadcastGenres()
        {
            libraryManager.forceBroadcastGenres();
        }

        public void broadcastAlbums()
        {
            libraryManager.forceBroadcastAlbums();
        }

        public void broadcastArtists()
        {
            libraryManager.forceBroadcastArtists();
        }

        public void broadcastSongs()
        {
            libraryManager.forceBroadcastSongs();
        }

        internal void BroadcastDirectories()
        {
            directoryScanner.ForceBroadcastDirectories();
        }
    }
}
