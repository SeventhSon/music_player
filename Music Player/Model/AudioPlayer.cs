using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Data;
using NAudio;
using NAudio.Wave;
using System.IO;
using System.Windows;
using System.Collections;

namespace Music_Player.Model
{
    public class AudioPlayer
    {
        IWavePlayer waveOutDevice;
        WaveStream mainOutputStream;
        WaveChannel32 volumeStream;
        private List<SongModel> queue;
        private int index=-1;
        private float Volume = 0.75f;
        public AudioPlayer()
        {
            waveOutDevice = new WaveOut();
            waveOutDevice.PlaybackStopped += OnPlaybackStopped;
        }
        public void ChangeVolume(int volume)
        {
            if (volumeStream != null)
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
            if (fileName.EndsWith(".mp3")&& File.Exists(fileName))
            {
                WaveStream mp3Reader = new Mp3FileReader(fileName);
                inputStream = new WaveChannel32(mp3Reader);
            }
            else
            {
                MessageBox.Show("File is missing!");
                return null;
            }
            volumeStream = inputStream;
            volumeStream.Volume = Volume;
            volumeStream.PadWithZeroes = false;
            return volumeStream;
        }
        private void ReloadTrack()
        {
            if (queue == null || queue.Count <= Index)
                return;
            CloseTrack();
            Artist = queue[Index].Artist;
            Track = queue[Index].Title;
            Album = queue[Index].Album;
            mainOutputStream = CreateInputStream(queue[Index].Path);
            if (mainOutputStream == null) return;
            waveOutDevice.Init(mainOutputStream);
            Index++;
            Play();
            GalaSoft.MvvmLight.Messaging.Messenger.Default.Send<string, ViewModel.ApplicationViewModel>("ReloadTrack");
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
        
        public void SetQueue(List<SongModel> q, int i)
        {
            CloseTrack();
            //queue = q.Copy();
            queue = q;
            Index = i;
            ReloadTrack();
        }
        void OnPlaybackStopped(object sender, EventArgs e)
        {
            ReloadTrack();
        }
        public int Index 
        {
            get { return index; } 
            private set 
            {
                if (value >= 0)
                {
                    index = value % queue.Count;
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
