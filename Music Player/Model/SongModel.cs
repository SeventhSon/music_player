using System.Windows.Controls;

namespace Music_Player.Model
{
    public class SongModel
    {
        public string Title { get; set; }
        public string Artist { get; set; }
        public string Album { get; set; }
        public int Year { get; set; }
        public string Genre { get; set; }
        public int Length 
        {
            get 
            {
                return Length;
            }
            set 
            { 
                Length = value;
                LengthString = Length % 60 < 10 ? Length / 60 + ":0" + Length % 60 : Length / 60 + ":" + Length % 60;
            } 
        }
        public string LengthString { get; set; }
        public string Path { get; set; }
        public int Rating { get; set; }
        public Image AlbumArt { get; set; }
        public int DirectoryID { get; set; }
        public int TrackNo { get; set; }
    }
}
