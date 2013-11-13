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
        private int _trackno;
        private int _directoryid;
        private int _rating;
        private string _genre;
        private int _year;
        private string _album;
        private string _artist;
        private string _title;
        public SongModel()
        { NowPlaying = false; }

        /// <summary>
        /// Class constructor setting track info from database
        /// </summary>
        /// <param name="row">Song row from DataRow</param>
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

        /// <summary>
        /// Gets and sets title
        /// </summary>
        public string Title
        {
            get
            {
                return _title;
            }
            set
            {
                _title = value;
                OnPropertyChanged("Title");
            }
        }

        /// <summary>
        /// Gets and sets artist
        /// </summary>
        public string Artist
        {
            get
            {
                return _artist;
            }
            set
            {
                _artist = value;
                OnPropertyChanged("Artist");
            }
        }

        /// <summary>
        /// Gets and sets album
        /// </summary>
        public string Album
        {
            get
            {
                return _album;
            }
            set
            {
                _album = value;
                OnPropertyChanged("Album");
            }
        }

        /// <summary>
        /// Gets and sets year
        /// </summary>
        public int Year
        {
            get
            {
                return _year;
            }
            set
            {
                _year = value;
                OnPropertyChanged("Year");
            }
        }

        /// <summary>
        /// Gets and sets genre
        /// </summary>
        public string Genre
        {
            get
            {
                return _genre;
            }
            set
            {
                _genre = value;
                OnPropertyChanged("Genre");
            }
        }

        /// <summary>
        /// Gets and sets track length in proper time format
        /// </summary>
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
                OnPropertyChanged("Length");
                OnPropertyChanged("LengthString");
            } 
        }

        /// <summary>
        /// Gets and sets length string
        /// </summary>
        public string LengthString { get; set; }

        /// <summary>
        /// Gets and sets path
        /// </summary>
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
                OnPropertyChanged("Path");
            }
        }

        /// <summary>
        /// Gets and sets rating
        /// </summary>
        public int Rating
        {
            get
            {
                return _rating;
            }
            set
            {
                _rating = value;
                OnPropertyChanged("NowPlaying");
            }
        }

        /// <summary>
        /// Gets and sets directory ID
        /// </summary>
        public int DirectoryID
        {
            get
            {
                return _directoryid;
            }
            set
            {
                _directoryid = value;
                OnPropertyChanged("DirectoryID");
            }
        }

        /// <summary>
        /// Gets and sets track number
        /// </summary>
        public int TrackNo
        {
            get
            {
                return _trackno;
            }
            set
            {
                _trackno = value;
                OnPropertyChanged("TrackNo");
            }
        }

        /// <summary>
        /// Gets and sets currently playing song
        /// </summary>
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

        /// <summary>
        /// Event handler for property changed
        /// </summary>
        /// <param name="name">Name of property changed</param>
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
