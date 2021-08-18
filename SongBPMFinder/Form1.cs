using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using Exocortex.DSP;

namespace SongBPMFinder
{
    public partial class Form1 : Form
    {
        TimingPointList currentTimingResult;
        AudioPlaybackSystem audioPlaybackSystem;

        //move to an array of a custom Struct/class
        Button currentSpeedButton = null;

        public TimingPointList CurrentTimingResult {
            get { return currentTimingResult; }
            set {
                currentTimingResult = value;
                audioViewer.TimingPoints = currentTimingResult;
            }
        }

        public Form1()
        {
            InitializeComponent();

            Logger.SetOutput(new RichTextBoxLogger(textOutput));

            audioPlaybackSystem = new AudioPlaybackSystem();
            audioViewer.LinkPlaybackSystem(audioPlaybackSystem);
            audioPlaybackSystem.OnNewSongLoad += AudioPlaybackSystem_OnNewSongLoad;
            audioPlaybackSystem.OnPositionChanged += AudioPlaybackSystem_OnPositionChanged;
            audioPlaybackSystem.OnSongPlay += AudioPlaybackSystem_OnSongPlay;
            audioPlaybackSystem.OnSongPause += AudioPlaybackSystem_OnSongPause;

            Plotting.LinkPlottingGraph(debugPlot1, testWaveformTab);
            Plotting.LinkPlottingGraph(debugPlot2, testWaveformTab2);
            Plotting.LinkPlottingGraph(debugPlot3, testWaveformTab3);
            Plotting.LinkPlottingGraph(debugPlot4, testWaveformTab4);
            Plotting.LinkPlottingGraph(debugPlot5, testWaveformTab5);

            applyVisualChangesToSpeedButton(buttonSpeed1x);

            audioPlaybackSystem.LoadFile("D:\\Archives\\Music\\Test\\Test0-5.mp3");
        }

        private void AudioPlaybackSystem_OnSongPause()
        {
            playPauseButton.Text = "4";
        }

        private void AudioPlaybackSystem_OnSongPlay()
        {
            playPauseButton.Text = ";";
        }

        FourierAudioDifferentiator dx = new FourierAudioDifferentiator(1024, 512);

        //TODO: remove this
        private void AudioPlaybackSystem_OnPositionChanged()
        {
            float[] fourierTransformData = new float[dx.SampleWindow];

            AudioData currentAudioFile = audioPlaybackSystem.CurrentAudioFile;
            AudioChannel currentFile = currentAudioFile[0];
            int windowLength = fourierTransformData.Length;
            int start = currentAudioFile.CurrentSample;
            int end = Math.Min(currentAudioFile.CurrentSample + windowLength, currentFile.Length);
            Span<float> window = currentFile.GetSlice(start, end);
            FourierTransform.FFTForwardsMagnitudes(window, fourierTransformData);
            Plotting.Plot(0, "Fourier transform", new AudioChannel(fourierTransformData, 44100));
        }


        private void AudioPlaybackSystem_OnNewSongLoad()
        {
            TimeSeries s1 = testDifferentiatorSettings(1024, 256, Color.Pink);
            TimeSeries s2 = testDifferentiatorSettings(1024, 512, Color.Red);
            TimeSeries s3 = testDifferentiatorSettings(1024, 768, Color.Yellow);
            TimeSeries s4 = testDifferentiatorSettings(1024, 1024,Color.Orange);
            TimeSeries s5 = testDifferentiatorSettings(1024, 2048, Color.Green);

            /*
            TimeSeries envelope = TimeSeries.CalculateEnvelope(s1, s2, s3, s4);
            envelope.Color = Color.Black;
            envelope.Width = 3;
            addTimeSeries(envelope);
            */
        }

        private TimeSeries testDifferentiatorSettings(int sampleWindow, int stride, Color c)
        {
            FourierAudioDifferentiator firstSetting = new FourierAudioDifferentiator(sampleWindow, stride);
            TimeSeries series = firstSetting.Differentiate(audioPlaybackSystem.CurrentAudioFile[0]);
            series.Normalize();
            series.Color = c;

            addTimeSeries(series);

            return series;
        }

        private void addTimeSeries(TimeSeries series)
        {
            audioViewer.AddTimeSeries(series);
        }

        private void openAudioForTimingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            audioPlaybackSystem.ShowOpenFilePrompt();
        }


        private void playPauseButton_Click(object sender, EventArgs e)
        {
            playPause();
        }

        bool isPressing = false;
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (isPressing)
                return;

            bool handled = true;

            if (e.KeyCode == Keys.Space)
            {
                playPauseButton.Focus();
                playPause();
            }
            else if(e.KeyCode == Keys.Left)
            {
                audioViewer.ScrollAudio(-1);
            }
            else if (e.KeyCode == Keys.Right)
            {
                audioViewer.ScrollAudio(1);
            }
            else
            {
                handled = false;
            }

            if (handled)
            {
                e.Handled = true;
            }
        }

        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
            isPressing = false;
        }

        void playPause()
        {
            audioPlaybackSystem.PlayPause();
        }


        void copyToClipboard(string s)
        {
            Clipboard.SetText(s);
            Logger.Log("Copied the following to the clipboard:\n\"" + s + "\n\"");
        }


        void applyVisualChangesToSpeedButton(Button b)
        {
            if (currentSpeedButton != null)
            {
                currentSpeedButton.BackColor = SystemColors.Control;
            }

            currentSpeedButton = b;
            b.BackColor = Color.Gray;
        }

        private void buttonSpeed1x_Click(object sender, EventArgs e)
        {
            audioPlaybackSystem.Playback = PlaybackRate.Realtime;
            applyVisualChangesToSpeedButton(buttonSpeed1x);
        }

        private void buttonSpeed075x_Click(object sender, EventArgs e)
        {
            audioPlaybackSystem.Playback = PlaybackRate.ThreeFourths;
            applyVisualChangesToSpeedButton(buttonSpeed075x);

        }

        private void buttonSpeed050x_Click(object sender, EventArgs e)
        {
            audioPlaybackSystem.Playback = PlaybackRate.Halftime;
            applyVisualChangesToSpeedButton(buttonSpeed050x);
        }

        private void buttonSpeed025x_Click(object sender, EventArgs e)
        {
            audioPlaybackSystem.Playback = PlaybackRate.Quartertime;
            applyVisualChangesToSpeedButton(buttonSpeed025x);
        }

        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Logger.Clear();
        }

        private void copyToClipboardToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            copyToClipboard(Logger.GetText());
        }

        private void calculateTimingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            calculateTiming();
        }

        void calculateTiming()
        {
            DateTime t = DateTime.Now;

            Logger.Log("Calculating timing...");
            CurrentTimingResult = TimingUtil.GetTiming(
                audioPlaybackSystem.CurrentAudioFile,
                new DefaultBeatDetector(),
                new TestTimingGenerator()
            );

            TimeSpan delta = DateTime.Now - t;
            Logger.Log("Calculated timing in " + delta.TotalMilliseconds + " ms");
        }

        private void osuTimingPointsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CurrentTimingResult == null)
            {
                calculateTiming();
            }

            string osuFormattedTimingPoints = new OsuTimingPointFormatter().FormatTiming(CurrentTimingResult);
            copyToClipboard(osuFormattedTimingPoints);
        }

        private void xMLFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Logger.Log("This feature has not yet been implemented");
        }

        
    }
}
