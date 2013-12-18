using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAudio.Wave;
using Music_Player.LibraryServiceReference;
using System.IO;
using System.Threading;
using Music_Player.Messaging;
using System.Windows.Threading;
using Music_Player.ViewModel;

namespace Music_Player.Model
{
    class StreamedAudioPlayer
    {
        enum StreamingPlaybackState
        {
            Stopped,
            Playing,
            Buffering,
            Paused
        }

        private LibraryServiceClient lsc;
        private BufferedWaveProvider bufferedWaveProvider;
        private IWavePlayer waveOut;
        private volatile StreamingPlaybackState playbackState;
        private volatile bool fullyDownloaded;
        private VolumeWaveProvider16 volumeProvider;
        private Thread StreamReader;
        private DispatcherTimer timer;
        
        private List<SongModel> queue;
        private int index = -1;
        private float Volume = 0.75f;

        private bool IsBufferNearlyFull
        {
            get
            {
                return bufferedWaveProvider != null &&
                       bufferedWaveProvider.BufferLength - bufferedWaveProvider.BufferedBytes
                       < bufferedWaveProvider.WaveFormat.AverageBytesPerSecond / 4;
            }
        }

        public StreamedAudioPlayer()
        {
            lsc = new LibraryServiceClient();
            StreamReader = null;
            waveOut = new WaveOut();
            waveOut.PlaybackStopped += OnPlaybackStopped;
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(350);
            timer.Tick += TimerTick;
        }
        public void ChangeVolume(int volume)
        {
            Volume = (float)volume / 100;
            if (volumeProvider != null)
                volumeProvider.Volume = Volume;
        }
        public void Play()
        {
            if (bufferedWaveProvider != null)
            {
                waveOut.Play();
                playbackState = StreamingPlaybackState.Playing;
            }
        }

        public void Pause()
        {
            if (bufferedWaveProvider != null)
            {
                playbackState = StreamingPlaybackState.Buffering;
                waveOut.Pause();
            }
        }
        public void Seek(float percentage)
        {
            stopCurrentDownload();
            playbackState = StreamingPlaybackState.Buffering;
            StreamReader = new Thread(() => readMP3FromStream(queue[Index].Path, percentage));
            StreamReader.Start();
        }
        public void Next()
        {
            if (bufferedWaveProvider != null)
            {
                Index++;
                stopCurrentDownload();
                playbackState = StreamingPlaybackState.Buffering;
                StreamReader = new Thread(() => readMP3FromStream(queue[Index].Path, 0f));
                StreamReader.Start();
            }
        }
        public void Prev()
        {
            if (bufferedWaveProvider != null)
            {
                Index--;
                stopCurrentDownload();
                playbackState = StreamingPlaybackState.Buffering;
                StreamReader = new Thread(() => readMP3FromStream(queue[Index].Path, 0f));
                StreamReader.Start();
            }
        }
        public void SetQueue(List<SongModel> q, int i)
        {
            queue = new List<SongModel>(q);
            Index = i;
            stopCurrentDownload();
            playbackState = StreamingPlaybackState.Buffering;
            StreamReader = new Thread(() => readMP3FromStream(queue[Index].Path, 0f));
            StreamReader.Start();
        }

        private void stopCurrentDownload()
        {
            if (StreamReader != null)
            {
                playbackState = StreamingPlaybackState.Stopped;
                StreamReader.Join();
                waveOut.Stop();
                bufferedWaveProvider = null;
                timer.Stop();
            }
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
                else if (value * -1 < queue.Count)
                    index = queue.Count - value;
                ForceNowPlayingBroadcast();
            }
        }
        public void ForceNowPlayingBroadcast()
        {
            if (queue != null)
            {
                NowPlayingPacket packet = new NowPlayingPacket(queue[Index]);
                GalaSoft.MvvmLight.Messaging.Messenger.Default.Send<NowPlayingPacket>(packet);
            }
        }
        private void readMP3FromStream(string path,float percentage)
        {
            fullyDownloaded = false;
            var buffer = new byte[16384 * 4]; // needs to be big enough to hold a decompressed frame

            IMp3FrameDecompressor decompressor = null;
            try
            {
                using (MemoryStream responseStream = new MemoryStream(lsc.GetSongStream(path,percentage)))
                {
                    ReadFullyStream readFullyStream = new ReadFullyStream(responseStream);
                    do
                    {
                        if (IsBufferNearlyFull)
                        {
                            Thread.Sleep(500);
                        }
                        else
                        {
                            Mp3Frame frame;
                            try
                            {
                                frame = Mp3Frame.LoadFromStream(readFullyStream);
                            }
                            catch (EndOfStreamException)
                            {
                                fullyDownloaded = true;
                                // reached the end of the MP3 file / stream
                                break;
                            }
                            if (frame == null)
                            {
                                fullyDownloaded = true;
                                break;
                            }
                            if (decompressor == null)
                            {
                                // don't think these details matter too much - just help ACM select the right codec
                                // however, the buffered provider doesn't know what sample rate it is working at
                                // until we have a frame
                                decompressor = CreateFrameDecompressor(frame);
                                bufferedWaveProvider = new BufferedWaveProvider(decompressor.OutputFormat);
                                bufferedWaveProvider.BufferDuration = TimeSpan.FromSeconds(20); // allow us to get well ahead of ourselves
                                createWaveOut();
                                //this.bufferedWaveProvider.BufferedDuration = 250;
                            }
                            int decompressed = decompressor.DecompressFrame(frame, buffer, 0);
                            //Debug.WriteLine(String.Format("Decompressed a frame {0}", decompressed));
                            bufferedWaveProvider.AddSamples(buffer, 0, decompressed);
                        }

                    } while (playbackState != StreamingPlaybackState.Stopped);
                    decompressor.Dispose();
                }
            }
            finally
            {
                if (decompressor != null)
                {
                    decompressor.Dispose();
                }
            }
        }

        private void createWaveOut()
        {
            volumeProvider = new VolumeWaveProvider16(bufferedWaveProvider);
            volumeProvider.Volume = Volume;
            waveOut.Init(volumeProvider);
            timer.Start();
        }
        private static IMp3FrameDecompressor CreateFrameDecompressor(Mp3Frame frame)
        {
            WaveFormat waveFormat = new Mp3WaveFormat(frame.SampleRate, frame.ChannelMode == ChannelMode.Mono ? 1 : 2,
                frame.FrameLength, frame.BitRate);
            return new AcmMp3FrameDecompressor(waveFormat);
        }
        private void OnPlaybackStopped(object sender, StoppedEventArgs e)
        {
            //playbackState = StreamingPlaybackState.Buffering;
            //StreamReader = new Thread(() => readMP3FromStream(queue[++Index].Path, 0f));
            //StreamReader.Start();
        }
        private void TimerTick(object sender, EventArgs e)
        {
            ViewModelLocator vm = new ViewModelLocator();
            if (playbackState != StreamingPlaybackState.Stopped)
            {
                if (bufferedWaveProvider != null)
                {
                    var bufferedSeconds = bufferedWaveProvider.BufferedDuration.TotalSeconds;
                    if (bufferedSeconds < 0.5 && playbackState == StreamingPlaybackState.Playing && !fullyDownloaded)
                    {
                        
                        vm.Application.IsPlaying = false;
                    }
                    else if (bufferedSeconds > 4 && playbackState == StreamingPlaybackState.Buffering)
                    {
                        vm.Application.IsPlaying = true;
                    }
                    else if (fullyDownloaded && bufferedSeconds == 0)
                    {
                        Next();
                    }
                }

            }
        }
    }
}
