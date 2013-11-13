using System;
using System.ComponentModel;
using System.Data;
using System.Linq;
namespace Music_Player.Model
{
    public class SongModel : INotifyPropertyChanged
    {
        private int _length;
        private  string _path;
        private bool _nowPlaying;
        public event PropertyChangedEventHandler PropertyChanged;
        public SongModel()
        { NowPlaying = false; }
        public SongModel(DataRow row)
        {
            Title = row["title"].ToString();
            Artist = row["artist"].ToString();
            Album = row["album"].ToString();
            Year = Int32.Parse(row["year"].ToString());
            Genre = row["genre"].ToString();
            Length = Int32.Parse(row["length"].ToString());
            Path = row["path"].ToString();
            DirectoryID = Int32.Parse(row["id_directory"].ToString());
            TrackNo = Int32.Parse(row["track_no"].ToString());
            NowPlaying = false;
        }
        public string Title { get; set; }
        public string Artist { get; set; }
        public string Album { get; set; }
        public int Year { get; set; }
        public string Genre { get; set; }
        public int Length 
        {
            get 
            {
                return _length;
            }
            set 
            { 
                _length = value;
                LengthString = _length % 60 < 10 ? _length / 60 + ":0" + _length % 60 : _length / 60 + ":" + _length % 60;
            } 
        }
        public string LengthString { get; set; }
        public string Path
        {
            get
            {
                return _path;
            }
            set
            {
                _path = value;
                if ((Title == null || Title.Equals("")) && _path!=null)
                    Title = _path.Split('\\').Last();
            }
        }
        public int Rating { get; set; }
        public int DirectoryID { get; set; }
        public int TrackNo { get; set; }
        public bool NowPlaying
        {
            get
            {
                return _nowPlaying;
            }
            set
            {
                _nowPlaying = value;
                OnPropertyChanged("NowPlaying");
            }
        }
          protected void OnPropertyChanged(string name)
          {
          PropertyChangedEventHandler handler = PropertyChanged;
          if (handler != null)
          {
              handler(this, new PropertyChangedEventArgs(name));
          }
      }
    }
}
