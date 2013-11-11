using Music_Player.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Music_Player.Messaging
{
    public class NowPlayingPacket: SongModel
    {
        public NowPlayingPacket(SongModel sm)
        {
            Album = sm.Album;
            Artist = sm.Artist;
            DirectoryID = sm.DirectoryID;
            Genre = sm.Genre;
            Length = sm.Length;
            LengthString = sm.LengthString;
            Path = sm.Path;
            Rating = sm.Rating;
            Title = sm.Title;
            TrackNo = sm.TrackNo;
            Year = sm.Year;
        }
        public BitmapImage AlbumArt { get; set; }
    }
}
