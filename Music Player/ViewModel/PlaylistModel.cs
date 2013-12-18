using Music_Player.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Music_Player.LibraryServiceReference;

namespace Music_Player.ViewModel
{
    public class PlaylistModel
    {
        public string Title { get; set; }
        public List<SongModel> Songs { get; set; }
    }
}
