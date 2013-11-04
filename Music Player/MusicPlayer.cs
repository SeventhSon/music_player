using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Data;
using NAudio;
using NAudio.Wave;

namespace Music_Player
{
    public class MusicPlayer
    {
        IWavePlayer waveOutDevice;
        WaveStream mainOutputStream;
        WaveChannel32 volumeStream;
        private DataTable queue;
        private int time = 0;
        public MusicPlayer()
        {
            waveOutDevice = new WaveOut();
            Run();
        }
        private void Run()
        {
            while (queue != null && Index < queue.Rows.Count)
            {

            }
        }
        internal void changeVolume(int volume)
        {
            throw new NotImplementedException();
        }

        internal void Pause()
        {
            throw new NotImplementedException();
        }

        internal void Play()
        {
            throw new NotImplementedException();
        }

        public DataTable Queue 
        {
            get
            {
                return queue;
            }

            set
            {

            }
        }

        public int Index
        {
            get
            {
                return 0;
            }

            set
            {

            }
        }
        internal void Seek(int TimeEllapsed)
        {
            throw new NotImplementedException();
        }
    }
}
