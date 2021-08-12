using System;
using System.Windows.Forms;

namespace SongBPMFinder
{
    public partial class CustomWaveViewer : UserControl
    {
        AudioData audioData;
        AudioPlaybackSystem playbackSystem;

        public AudioData AudioData {
            get {
                return audioData;
            }
            set {
                audioData = value;
            }
        }

        public void LinkPlaybackSystem(AudioPlaybackSystem system)
        {
            playbackSystem = system;
            playbackSystem.WhileSongIsPlaying += PlaybackSystem_WhileSongIsPlaying;
            playbackSystem.OnNewSongLoad += PlaybackSystem_OnNewSongLoad;
        }


        private void PlaybackSystem_OnNewSongLoad()
        {
            AudioData = playbackSystem.CurrentAudioFile;
            viewport.AudioData = AudioData;

            UpdateScrollExtents();
        }
        void UpdateScrollExtents()
        {
            if (audioData == null)
                return;

            int windowLength = viewport.Coordinates.WindowLengthSamples;
            hScrollBar.Minimum = -windowLength / 2;
            hScrollBar.Maximum = Math.Max(0, audioData.Length - windowLength / 2);
            hScrollBar.Value = hScrollBar.Minimum + audioData.CurrentSample;
        }


        private void PlaybackSystem_WhileSongIsPlaying()
        {
            Invalidate();
        }

        public CustomWaveViewer()
        {
            InitializeComponent();

            MouseWheel += onMouseWheelScroll;
        }


        private void onMouseWheelScroll(object sender, MouseEventArgs e)
        {
            int dir = e.Delta > 1 ? 1 : -1;

            if (Control.ModifierKeys == Keys.Control)
            {
                Zoom(dir);
            }
            else
            {
                ScrollAudio(dir);
            }
        }

        private void Zoom(int dir)
        {
            viewport.Coordinates.Zoom(dir, 2.0f);
            Logger.Log("zoom: " + viewport.Coordinates.SecondsPerPixel + "s/px");
        }

        private void ScrollAudio(int dir)
        {
            if (ModifierKeys == Keys.Shift)
            {
                viewport.Coordinates.ScrollAudio(dir * 0.1f);
            }
            else
            {
                viewport.Coordinates.ScrollAudio(dir);
            }
        }

        private void onHScrollbarScroll(object sender, ScrollEventArgs e)
        {
            if (playbackSystem == null)
                return;

            playbackSystem.SeekSample(hScrollBar.Value - hScrollBar.Minimum);
            Invalidate();
        }
    }
}
