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
using Music_Player.Messaging;
using System.Drawing;
using System.Windows.Media.Imaging;

namespace Music_Player.Model
{
    public class AudioPlayer
    {
        private Object monitor = new Object();

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
            lock (monitor)
            {
                if (volumeStream != null)
                    volumeStream.Volume = (float)volume / 100;
                Volume = (float)volume / 100;
            }
        }
        public void Pause()
        {
            lock (monitor)
            {
                if (waveOutDevice.PlaybackState == PlaybackState.Playing && volumeStream != null)
                    waveOutDevice.Pause();
            }
        }

        public void Play()
        {
            lock (monitor)
            {
                if (waveOutDevice.PlaybackState != PlaybackState.Playing && volumeStream != null)
                    waveOutDevice.Play();
            }
        }
        public void Seek(int time)
        {
            lock (monitor)
            {
                if (volumeStream != null)
                    volumeStream.CurrentTime = new TimeSpan(0, 0, time);
            }
        }
        public void Next()
        {
            lock (monitor)
            {
                if (volumeStream != null)
                {
                    waveOutDevice.Stop();
                }
            }
        }
        public void Prev()
        {
            lock (monitor)
            {
                if (volumeStream != null && volumeStream.CurrentTime.TotalMilliseconds < 2500)
                {
                    Index -= 2;
                    waveOutDevice.Stop();
                }
                else if (volumeStream != null && volumeStream.CurrentTime.TotalMilliseconds >= 2500)
                {
                    Index--;
                    waveOutDevice.Stop();
                }
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
            Artist = queue[Index].Artist;
            Track = queue[Index].Title;
            Album = queue[Index].Album;
            mainOutputStream = CreateInputStream(queue[Index].Path);
            if (mainOutputStream == null) return;
            waveOutDevice.Init(mainOutputStream);
            Play();
            ForceNowPlayingBroadcast();
            Index++;
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
            lock (monitor)
            {
                queue = new List<SongModel>(q);
                Index = i;
                if (mainOutputStream == null)
                    ReloadTrack();
                else
                    CloseTrack();
            }
        }
        void OnPlaybackStopped(object sender, EventArgs e)
        {
            lock (monitor)
            {
                ReloadTrack();
            }
        }
        public int Index 
        {
            get { return index; } 
            private set 
            {
                lock (monitor)
                {
                    if (value >= 0)
                    {
                        index = value % queue.Count;
                    }
                    else if (value * -1 < queue.Count)
                        index = queue.Count - value;
                }
            } 
        }
        public void ForceNowPlayingBroadcast()
        {
            lock (monitor)
            {
                if (queue != null)
                {
                    NowPlayingPacket packet = new NowPlayingPacket(queue[Index]);
                    GalaSoft.MvvmLight.Messaging.Messenger.Default.Send<NowPlayingPacket>(packet);
                }
            }
        }
        public int GetTrackLength()
        {
            lock (monitor)
            {
                if (volumeStream != null)
                    return (int)volumeStream.TotalTime.TotalSeconds;
                return 0;
            }
        }
        public string Artist{get;private set;}
        public string Track { get; private set; }
        public string Album{get;private set;}
    }
}
