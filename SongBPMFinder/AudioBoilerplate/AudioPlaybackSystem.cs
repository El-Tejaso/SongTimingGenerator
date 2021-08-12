using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SongBPMFinder
{
    public class AudioPlaybackSystem
    {
        AudioData currentAudioFile = null;
        public AudioData CurrentAudioFile { get { return currentAudioFile; } }

        AudioDataStream audioStream;
        AudioPlayer player = new AudioPlayer();

        OpenFileDialog openFileDialog;

        Timer songIsPlayingTicker;

        public event Action WhileSongIsPlaying;
        public event Action OnSongPlay;
        public event Action OnSongPause;
        public event Action OnNewSongLoad;

        //It is important that this isn't invoked in AudioPlayer's CurrentSample's setter or similar
        //Because that will be called on an audio thread which might lead to race conditions and whatnot
        public event Action OnAudioScroll;

        public int CurrentSample {
            get {
                return currentAudioFile.CurrentSample;
            }
        }

        public Playback Playback {
            get {
                return audioStream.Playback;
            }
            set {
                audioStream.Playback = value;
            }
        }

        public AudioPlaybackSystem()
        {
            openFileDialog = new OpenFileDialog();

            songIsPlayingTicker = new Timer();
            songIsPlayingTicker.Stop();
            songIsPlayingTicker.Enabled = false;
            songIsPlayingTicker.Interval = 1000 / 30;
            songIsPlayingTicker.Tick += onSongPlayingTimerTick;
        }

        private void onSongPlayingTimerTick(object sender, EventArgs e)
        {
            WhileSongIsPlaying?.Invoke();
        }

        public void ShowOpenFilePrompt()
        {
            openFileDialog.Filter = "Audio Files(*.mp3, *.mp4)|*.mp3;*.mp4";
            openFileDialog.Title = "Open an audio file for timing";

            DialogResult res = openFileDialog.ShowDialog();

            if (res == DialogResult.OK)
            {
                LoadFile(openFileDialog.FileName);
            }
        }

        public bool LoadFile(string filename)
        {
            if (currentAudioFile != null)
            {
                currentAudioFile = null;
                audioStream = null;

                ///////////////////////////////////////////
                GC.Collect();
                ///////////////////////////////////////////
            }

            AudioData d = AudioData.FromFile(filename);
            if (d == null)
                return false;

            setCurrentAudio(d);

            OnNewSongLoad?.Invoke();

            return true;
        }

        void setCurrentAudio(AudioData audioData)
        {
            currentAudioFile = audioData;

            audioStream = new AudioDataStream(currentAudioFile);
            player.SetAudio(audioStream);
        }

        public void PlayPause()
        {
            if (currentAudioFile == null)
                return;

            if (player.IsPlaying)
            {
                Pause();
            }
            else
            {
                Play();
            }
        }

        public void Pause()
        {
            songIsPlayingTicker.Stop();
            player.Pause();
            OnSongPause?.Invoke();
        }

        public void Play()
        {
            songIsPlayingTicker.Start();
            player.Play();
            OnSongPlay?.Invoke();
        }

        public void SeekSample(int sample)
        {
            currentAudioFile.CurrentSample = sample;
            OnAudioScroll?.Invoke();
        }
    }
}
