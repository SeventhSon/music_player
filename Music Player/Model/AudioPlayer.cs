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
    /// <summary>
    /// Class responsible for playing songs and managing them
    /// </summary>
    public class AudioPlayer
    {
        private Object monitor = new Object();

        IWavePlayer waveOutDevice;
        WaveStream mainOutputStream;
        WaveChannel32 volumeStream;
        private List<SongModel> queue;
        private int index=-1;
        private float Volume = 0.75f;

        /// <summary>
        /// Class constructor
        /// </summary>
        public AudioPlayer()
        {
            waveOutDevice = new WaveOut();
            waveOutDevice.PlaybackStopped += OnPlaybackStopped;
        }

        /// <summary>
        /// Changes volume
        /// </summary>
        /// <param name="volume"></param>
        public void ChangeVolume(int volume)
        {
            lock (monitor)
            {
                if (volumeStream != null)
                    volumeStream.Volume = (float)volume / 100;
                Volume = (float)volume / 100;
            }
        }

        /// <summary>
        /// Pauses music
        /// </summary>
        public void Pause()
        {
            lock (monitor)
            {
                if (waveOutDevice.PlaybackState == PlaybackState.Playing && volumeStream != null)
                    waveOutDevice.Pause();
            }
        }

        /// <summary>
        /// Plays music
        /// </summary>
        public void Play()
        {
            lock (monitor)
            {
                if (waveOutDevice.PlaybackState != PlaybackState.Playing && volumeStream != null)
                    waveOutDevice.Play();
            }
        }

        /// <summary>
        /// Seeks music to a given time
        /// </summary>
        /// <param name="time">seek time</param>
        public void Seek(int time)
        {
            lock (monitor)
            {
                if (volumeStream != null)
                    volumeStream.CurrentTime = new TimeSpan(0, 0, time);
            }
        }

        /// <summary>
        /// Plays next song
        /// </summary>
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

        /// <summary>
        /// Plays previous song
        /// </summary>
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

        /// <summary>
        /// Opens music file and returns audio stream
        /// </summary>
        /// <param name="fileName">path to file</param>
        /// <returns></returns>
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

        /// <summary>
        /// Reloads track and all the info about it
        /// </summary>
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

        /// <summary>
        /// Stops the playing track, closes audio stream and any open music file
        /// </summary>
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
        
        /// <summary>
        /// Creates queue with songs from given list 
        /// </summary>
        /// <param name="q"> SongModel list</param>
        /// <param name="i"> Currently selected song</param>
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

        /// <summary>
        /// Reloads track when currently playing music finishes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnPlaybackStopped(object sender, EventArgs e)
        {
            lock (monitor)
            {
                ReloadTrack();
            }
        }

        /// <summary>
        /// Gets and sets index of currently choosen song
        /// </summary>
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

        /// <summary>
        /// Broadcast now playing packet with information about currently playing song
        /// </summary>
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

        /// <summary>
        /// Gets currently playing song length
        /// </summary>
        /// <returns></returns>
        public int GetTrackLength()
        {
            lock (monitor)
            {
                if (volumeStream != null)
                    return (int)volumeStream.TotalTime.TotalSeconds;
                return 0;
            }
        }

        /// <summary>
        /// Gets and sets Artist
        /// </summary>
        public string Artist{get;private set;}

        /// <summary>
        /// Gets and sets Track
        /// </summary>
        public string Track { get; private set; }

        /// <summary>
        /// Gets and sets Album
        /// </summary>
        public string Album{get;private set;}
    }
}
