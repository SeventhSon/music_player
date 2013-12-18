using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Music_Player.LibraryServiceReference;
using System.Threading;

namespace Music_Player.Model
{
    public class MusicPlayer
    {
        private static volatile MusicPlayer _instance;
        private static object monitor = new Object();
        private LibraryManager libraryManager;
        private StreamedAudioPlayer audioPlayer;
        private Thread Worker;
        private bool running = true;
        private List<InputEvent> InputQueue;
        private MusicPlayer()
        {
            libraryManager = new LibraryManager();
            audioPlayer = new StreamedAudioPlayer();
            InputQueue = new List<InputEvent>();
            Worker = new Thread(() => Run());
            Worker.Start();
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
        private void Run()
        {
            while(running)
            {
                List<InputEvent> iq;
                lock (monitor)
                {
                    iq = new List<InputEvent>(InputQueue);
                    InputQueue.Clear();
                }
                for(int i=0;i<iq.Count;i++)
                {
                    switch(iq[i].Type)
                    {
                        case InputEvent.ActionType.Broadcast:
                            libraryManager.ForceBroadcastPlaylists();
                            libraryManager.ForceBroadcastGenres();
                            libraryManager.ForceBroadcastAlbums();
                            libraryManager.ForceBroadcastArtists();
                            libraryManager.ForceBroadcastSongs();
                            break;
                        case InputEvent.ActionType.Play:
                            audioPlayer.Play();
                            break;
                        case InputEvent.ActionType.Pause:
                            audioPlayer.Pause();
                            break;
                        case InputEvent.ActionType.Next:
                            audioPlayer.Next();
                            break;
                        case InputEvent.ActionType.Prev:
                            audioPlayer.Prev();
                            break;
                        case InputEvent.ActionType.Seek:
                            audioPlayer.Seek((float)iq[i].Param);
                            break;
                        case InputEvent.ActionType.SetVolume:
                            audioPlayer.ChangeVolume((int)iq[i].Param);
                            break;
                        case InputEvent.ActionType.SetQueue:
                            audioPlayer.SetQueue((List<SongModel>)iq[i].Param, (int)iq[i].Param2);
                            break;
                    }
                }

                Thread.Sleep(250);
            }
        }
        public void addEvent(InputEvent.ActionType type, object param, object param2)
        {
            InputQueue.Add(new InputEvent(type, param, param2));
        }
    }
    public class InputEvent
    {
        public enum ActionType
        {
            Pause,
            Play,
            SetVolume,
            Seek,
            Next,
            Prev,
            Broadcast,
            SetQueue
        }
        public ActionType Type { get; set; }
        public Object Param { get; set; }

        public Object Param2 { get; set; }

        public InputEvent(ActionType type,object param, object param2)
        {
            Type = type;
            Param = param;
            Param2 = param2;
        }
    }
}
