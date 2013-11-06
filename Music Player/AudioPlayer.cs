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
    public class AudioPlayer
    {
        IWavePlayer waveOutDevice;
        WaveStream mainOutputStream;
        WaveChannel32 volumeStream;
        private DataTable queue;
        private int index=-1;
        private float Volume = 0.75f;
        public AudioPlayer()
        {
            waveOutDevice = new WaveOut();
            waveOutDevice.PlaybackStopped += OnPlaybackStopped;
        }
        public void ChangeVolume(int volume)
        {
            volumeStream.Volume = (float)volume / 100;
            Volume = (float)volume / 100;
        }
        public void Pause()
        {
            if (waveOutDevice.PlaybackState == PlaybackState.Playing && volumeStream!=null)
                waveOutDevice.Pause();
        }

        public void Play()
        {
            if (waveOutDevice.PlaybackState != PlaybackState.Playing && volumeStream != null)
                waveOutDevice.Play();
        }
        public void Seek(int time)
        {
            if (volumeStream != null)
                volumeStream.CurrentTime = new TimeSpan(0, 0, time);
        }
        public void Next()
        {
            if (volumeStream != null) 
            {
                //Index++;
                waveOutDevice.Stop();
            }
        }
        public void Prev()
        {
            if (volumeStream != null && volumeStream.CurrentTime.TotalMilliseconds < 2500)
            {
                Index-=2;
                waveOutDevice.Stop();
            }
            else if (volumeStream != null && volumeStream.CurrentTime.TotalMilliseconds >= 2500)
            {
                Index--;
                waveOutDevice.Stop();
            }
        }
        private WaveStream CreateInputStream(string fileName)
        {
            WaveChannel32 inputStream;
            if (fileName.EndsWith(".mp3"))
            {
                WaveStream mp3Reader = new Mp3FileReader(fileName);
                inputStream = new WaveChannel32(mp3Reader);
            }
            else
            {
                throw new InvalidOperationException("Unsupported extension");
            }
            volumeStream = inputStream;
            volumeStream.Volume = Volume;
            volumeStream.PadWithZeroes = false;
            return volumeStream;
        }
        private void Reload()
        {
            if (queue == null || queue.Rows.Count <= Index)
                return;
            CloseTrack();
            Artist = queue.Rows[Index]["Artist"].ToString();
            Track = queue.Rows[Index]["Title"].ToString();
            Album = queue.Rows[Index]["Album"].ToString();
            mainOutputStream = CreateInputStream(queue.Rows[Index]["Path"].ToString());
            waveOutDevice.Init(mainOutputStream);
            Index++;
            Play();
            GalaSoft.MvvmLight.Messaging.Messenger.Default.Send<string, ViewModel.MainViewModel>("ReloadTrack");
        }
        private void CloseTrack()
        {
            if (waveOutDevice != null)
            {
                waveOutDevice.Stop();
            }
            if (mainOutputStream != null)
            {
                // this one really closes the file and ACM conversion
                volumeStream.Close();
                volumeStream = null;
                // this one does the metering stream
                mainOutputStream.Close();
                mainOutputStream = null;
            }
        }
        
        public void SetQueue(DataTable q, int i)
        {
            CloseTrack();
            //queue = q.Copy();
            queue = q;
            Index = i;
            Reload();
        }
        void OnPlaybackStopped(object sender, EventArgs e)
        {
            Reload();
        }
        public int Index 
        {
            get { return index; } 
            private set 
            {
                if (value >= 0)
                {
                    index = value % queue.Rows.Count;
                }
            } 
        }
        public int GetTrackLength()
        {
            if (volumeStream != null)
                return (int) volumeStream.TotalTime.TotalSeconds;
            return 0;
        }
        public string Artist{get;private set;}
        public string Track { get; private set; }
        public string Album{get;private set;}
    }
}
