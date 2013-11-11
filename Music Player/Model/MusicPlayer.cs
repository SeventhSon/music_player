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
        private MusicPlayer()
        {

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
            //throw new NotImplementedException();
        }

        internal void setQueue(List<SongModel> SongList, int selectedIndex)
        {
            //throw new NotImplementedException();
        }

        internal void NextSong()
        {
            //throw new NotImplementedException();
        }

        internal void PrevSong()
        {
            //throw new NotImplementedException();
        }

        internal void PlaySong()
        {
            //throw new NotImplementedException();
        }

        internal void PauseSong()
        {
            //throw new NotImplementedException();
        }

        internal void ChangeSongVolume(int _volume)
        {
            //throw new NotImplementedException();
        }

        internal void SeekSong(int TimeEllapsed)
        {
            //throw new NotImplementedException();
        }

        internal void broadcastPlaylists()
        {
            //throw new NotImplementedException();
        }

        internal void broadcastInfo()
        {
            //throw new NotImplementedException();
        }

        internal void ScanDirectory(string p)
        {
            //throw new NotImplementedException();
        }

        internal void broadcastGenres()
        {
            //throw new NotImplementedException();
        }

        internal void broadcastAlbums()
        {
            //throw new NotImplementedException();
        }

        internal void broadcastArtists()
        {
            //throw new NotImplementedException();
        }

        internal void broadcastSongs()
        {
            //throw new NotImplementedException();
        }
    }
}
