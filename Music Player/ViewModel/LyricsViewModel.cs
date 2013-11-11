using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using Music_Player.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Music_Player.ViewModel
{
    public class LyricsViewModel: ViewModelBase
    {
        private string _title;
        private string _lyrics;
        public LyricsViewModel()
        {
            Messenger.Default.Register<InfoPacket>
            (
                this,
                (action) => ReceiveMessage(action)
            );
        }
        public string Title
        {
            get
            {
                return _title;
            }
            set
            {
                _title = value;
                RaisePropertyChanged("Title");
            }
        }
        public string Lyrics
        {
            get
            {
                return _lyrics;
            }
            set
            {
                _lyrics = value;
                RaisePropertyChanged("Lyrics");
            }
        }
        private void ReceiveMessage(InfoPacket packet)
        {
            Lyrics = packet.Lyrics;
            Title = packet.Title;
        }
    }
}
