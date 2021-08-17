using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

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

        //TODO: remove this
        private void AudioPlaybackSystem_OnPositionChanged()
        {

        }


        //TODO: remove this
        private void AudioPlaybackSystem_OnNewSongLoad()
        {
            TimeSeries series = new TimeSeries();
            Random r = new Random();
            AudioData currentAudioFile = audioPlaybackSystem.CurrentAudioFile;

            int windowSize = 1024;
            float[] lastFT = new float[windowSize/2];
            float[] thisFT = new float[windowSize/2];


            for (int i = 0; i + windowSize < currentAudioFile.Length; i+=windowSize/2)
            {
                Span<float> slice = currentAudioFile[0].GetSlice(i, i + windowSize);

                if(i == 0)
                {
                    FourierTransform.FourierTransformMagnitudes(slice, lastFT);
                    continue;
                }

                FourierTransform.FourierTransformMagnitudes(slice, thisFT);

                float sumSquaresDifference = 0;

                for(int j = 0; j < lastFT.Length; j++)
                {
                    float delta = thisFT[j] - lastFT[j];
                    sumSquaresDifference += delta * delta;
                }

                float[] temp = thisFT;
                thisFT = lastFT;
                lastFT = temp;

                double t = currentAudioFile.SampleToSeconds(i);
                series.Add(t, sumSquaresDifference);
            }

            series.Normalize();
            audioViewer.TimeSeries = series;
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
