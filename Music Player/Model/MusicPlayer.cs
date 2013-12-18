using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Music_Player.LibraryServiceReference;

namespace Music_Player.Model
{
    public class MusicPlayer
    {
        private static volatile MusicPlayer _instance;
        private static object monitor = new Object();
        private LibraryManager libraryManager;
        private StreamedAudioPlayer audioPlayer;
        private InfoScrapper infoScrapper;
        private MusicPlayer()
        {
            libraryManager = new LibraryManager();
            audioPlayer = new StreamedAudioPlayer();
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
        public void BroadcastNowPlaying()
        {
            audioPlayer.ForceNowPlayingBroadcast();
        }

        public void setQueue(List<SongModel> SongList, int selectedIndex)
        {
            audioPlayer.SetQueue(SongList, selectedIndex);
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
    }
}
