using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using GalaSoft.MvvmLight.Messaging;
using Music_Player.ViewModel;
using System.IO;
using Music_Player.Messaging;
using System.Threading;

namespace Music_Player.Model
{
    class LibraryManager
    {
        /// <summary>
        /// Create new object of Library Manager and rescans every already scanned directory
        /// </summary>
        /// 
        private object filelock = new Object();
        private DBManager dbm;
        private string nowPlayingPath ="";
        public LibraryManager()
        {
            dbm = DBManager.Instance;
        }

        public void ForceBroadcastPlaylists()
        {
            //throw new NotImplementedException();
        }

        public void ForceBroadcastGenres()
        {
            //throw new NotImplementedException();
        }

        public void ForceBroadcastSongs()
        {
            DataTable dt = dbm.ExecuteQuery("Select * from songs");
            List<SongModel> packet = new List<SongModel>();
            foreach (DataRow row in dt.Rows)
            {
                SongModel song = new SongModel(row);
                if(song.Path.Equals(nowPlayingPath))
                    song.NowPlaying = true;
                packet.Add(song);
            }
            GalaSoft.MvvmLight.Messaging.Messenger.Default.Send<List<SongModel>>(packet);    
        }

        public void ForceBroadcastArtists()
        {
            //throw new NotImplementedException();
        }

        public void ForceBroadcastAlbums()
        {
            //throw new NotImplementedException();
        }
        public void SaveSongData(List<SongModel> SongsToSave)
        {
                foreach (SongModel song in SongsToSave)
                {
                    string update = "";
                    update += "title=\"" + song.Title + "\", album=\"" + song.Album + "\", artist=\"" + song.Artist +
                        "\", year=" + song.Year + ", genre=\"" + song.Genre + "\", rating=" + song.Rating + " where path=\"" + song.Path + "\"";
                    dbm.ExecuteNonQuery("update or replace songs  set " + update);
                    TagLib.File tags = TagLib.File.Create(song.Path);
                    tags.Tag.Album = song.Album;
                    tags.Tag.AlbumArtists = song.Artist.Split(',');
                    tags.Tag.Title = song.Title;
                    tags.Tag.Year = (uint)song.Year;
                    tags.Tag.Genres = song.Genre.Split(',');
                    try
                    {
                        tags.Save();
                    }catch(System.IO.IOException e)
                    {
                        Thread.Sleep(1000);
                        SaveSongData(SongsToSave);
                    }

                }
                ForceBroadcastSongs();
        }
        private void ReceiveMessage(NowPlayingPacket packet)
        {
            nowPlayingPath = packet.Path;
        }
    }
}
