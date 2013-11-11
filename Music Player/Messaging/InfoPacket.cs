using Music_Player.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Music_Player.Messaging
{
    public class InfoPacket
    {
        public string Description { set; get; }
        public string Lyrics { set; get; }
        public string Title { set; get; }
        public List<PhotoModel> Photos { set; get; }
    }
}
