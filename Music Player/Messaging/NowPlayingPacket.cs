using Music_Player.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace Music_Player.Messaging
{
    public class NowPlayingPacket: SongModel
    {
        private static BitmapImage defaultImg;
        private BitmapImage _albumArt;
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
            if (defaultImg == null)
            {
                defaultImg = new BitmapImage();
                defaultImg.BeginInit();
                defaultImg.UriSource = new Uri("./Images/albumart.jpg", UriKind.Relative);
                defaultImg.CacheOption = BitmapCacheOption.OnLoad;
                defaultImg.EndInit();
                defaultImg.Freeze();
            }
            var file = TagLib.File.Create(Path);
            if (file.Tag.Pictures.Length >= 1)
            {
                BitmapImage bi = new BitmapImage();
                using (MemoryStream ms = new MemoryStream(file.Tag.Pictures[0].Data.Data))
                {
                    bi.BeginInit();
                    bi.StreamSource = ms;
                    bi.CacheOption = BitmapCacheOption.OnLoad;
                    bi.EndInit();
                }
                bi.Freeze();
                AlbumArt = bi;
            }
        }
        public BitmapImage AlbumArt
        {
            get
            {
                if (_albumArt == null)
                    return defaultImg;
                return _albumArt;
            }
            set
            {
                if (value != null)
                    _albumArt = value;
            }
        }
    }
}
