using System;
using System.Windows.Forms;

namespace SongBPMFinder
{
    public partial class CustomWaveViewer : UserControl
    {
        AudioData audioData;
        AudioPlaybackSystem playbackSystem;

        public TimingPointList TimingPoints {
            get { return viewport.TimingPoints; }
            set { viewport.TimingPoints = value; }
        }

        public void AddTimeSeries(TimeSeries t)
        {
            viewport.AddTimeSeries(t);
        }

        public void RemoveTimeSeries(TimeSeries t)
        {
            viewport.RemoveTimeSeries(t);
        }

        public AudioData AudioData {
            get {
                return audioData;
            }
            set {
                if(audioData != value)
                {
                    if(audioData != null)
                    {
                        audioData.OnPositionManuallyChanged -= AudioData_OnPositionManuallyChanged;
                    }

                    audioData = value;
                    viewport.AudioData = value;

                    if (audioData != null)
                    {
                        audioData.OnPositionManuallyChanged += AudioData_OnPositionManuallyChanged;
                    }
                }
            }
        }

        private void AudioData_OnPositionManuallyChanged()
        {
            Invalidate();
        }

        public CustomWaveViewer()
        {
            InitializeComponent();

            MouseWheel += onMouseWheelScroll;
        }


        public void LinkPlaybackSystem(AudioPlaybackSystem system)
        {
            playbackSystem = system;
            playbackSystem.OnPositionChanged += PlaybackSystem_OnPositionChanged;
            playbackSystem.OnNewSongLoad += PlaybackSystem_OnNewSongLoad;
        }


        private void PlaybackSystem_OnNewSongLoad()
        {
            AudioData = playbackSystem.CurrentAudioFile;
            
            UpdateScrollExtents();
        }

        void UpdateScrollExtents()
        {
            if (audioData == null)
                return;

            int windowLength = viewport.Coordinates.WindowLengthSamples;
            hScrollBar.Minimum = -windowLength / 2;
            hScrollBar.Maximum = Math.Max(0, audioData.Length + windowLength / 2);
            hScrollBar.Value = hScrollBar.Minimum + audioData.CurrentSample;
        }


        private void PlaybackSystem_OnPositionChanged()
        {
            Invalidate();
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

            UpdateScrollExtents();

            Invalidate();
        }

        public void ScrollAudio(int dir)
        {
            if (audioData == null)
                return;

            float amount;
            if (ModifierKeys == Keys.Shift)
            {
                amount = (dir * 0.1f);
            }
            else
            {
                amount = (dir);
            }

            int windowLengthInSamples = viewport.Coordinates.WindowLengthSamples;
            int newPosition = audioData.CurrentSample - (int)(amount * windowLengthInSamples / 20);
            audioData.SetCurrentSampleWithEvent(newPosition);


            hScrollBar.Value = audioData.CurrentSample;

            Invalidate();
        }

        private void onHScrollbarScroll(object sender, ScrollEventArgs e)
        {
            seekSample(hScrollBar.Value - hScrollBar.Minimum);
        }

        private void seekSample(int sample)
        {
            if (audioData == null)
                return;

            audioData.SetCurrentSampleWithEvent(sample);
            Invalidate();
        }
    }
}
