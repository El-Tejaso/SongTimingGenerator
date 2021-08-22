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
        AudioPlaybackSystem audioPlaybackSystem;

        TimingPointList currentTimingResult;
        TimingPipeline timingPipeline;

        bool timeWithActiveWindow {
            get {
                return localizedTimingCheckbox.Checked;
            }
        }


        //move to an array of a custom Struct/class
        Button currentSpeedButton = null;

        public Form1()
        {
            InitializeComponent();

            Logger.SetOutput(new RichTextBoxLogger(textOutput));
            audioPlaybackSystem = new AudioPlaybackSystem();
            timingPipeline = new TimingPipeline();

            Logger.Log("Press Ctrl+Shift+R to quickly recalculate the timing. " +
                "The program is in it's early days so it will be extremely slow. " +
                "Nor is there a progress-bar. " +
                "sorry about that");

            InitializePipelineParameters();

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

        private void InitializePipelineParameters()
        {
            fourierWindowCombobox.SelectedIndex = 2;
            addAllFreqCheckbox.Checked = false;
            leftChannelCheckbox.Checked = true;
            rightChannelCheckbox.Checked = true;
            evalDistanceNumeric.Value = (decimal)0.01;
            binaryPeakCheckbox.Checked = false;
            numFreqBandsNumeric.Value = 1;
            strideNumeric.Value = 0.0005M;
            differenceFunctionCombobox.SelectedIndex = 0;

            FourierWindowCombobox_OnSelectedIndexChanged(null, null);
            AddAllFrequ_OnCheckChanded(null, null);
            leftChannelLeckbox_CheckedChanged(null, null);
            rightChannelCheckbox_CheckedChanged(null, null);
            evalDistanceNumeric_OnValueChanged(null, null);
            binaryPeakCheckbox_CheckedChanged(null, null);
            numFrequNumeric_OnValueChanged(null, null);
            strideNumeric_ValueChanged(null, null);
            differenceFunctionCombobox_SelectedIndexChanged(null, null);
        }

        private void AudioPlaybackSystem_OnSongPause()
        {
            playPauseButton.Text = "4";
        }

        private void AudioPlaybackSystem_OnSongPlay()
        {
            playPauseButton.Text = ";";
        }


        private void AudioPlaybackSystem_OnPositionChanged()
        {

        }


        private void AudioPlaybackSystem_OnNewSongLoad()
        {
        }


        private void addTimeSeries(TimeSeries series)
        {
            audioViewer.AddDrawable(new DrawableTimeSeries(series));
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


        private void osuTimingPointsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (currentTimingResult == null)
            {
                calculateTiming();
            }

            string osuFormattedTimingPoints = new OsuTimingPointFormatter().FormatTiming(currentTimingResult);
            copyToClipboard(osuFormattedTimingPoints);
        }

        private void xMLFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Logger.Log("This feature has not yet been implemented");
        }


        void calculateTiming()
        {
            if (localizedTimingCheckbox.Checked)
            {
                timingPipeline.Start = audioViewer.Coordinates.WindowLeftSeconds;
                timingPipeline.End = audioViewer.Coordinates.WindowRightSeconds;
            }
            else
            {
                timingPipeline.Start = -1;
                timingPipeline.End = -1;
            }


            DateTime t = DateTime.Now;

            Logger.Log("Calculating timing...");

            currentTimingResult = timingPipeline.TimeSong(audioPlaybackSystem.CurrentAudioFile);

            TimeSpan delta = DateTime.Now - t;
            Logger.Log("Calculated timing in " + delta.TotalMilliseconds + " ms");


            audioViewer.ClearDrawables();


            for (int i = 0; i < timingPipeline.DebugTimeSeries.Count; i++)
            {
                addTimeSeries(timingPipeline.DebugTimeSeries[i]);
            }
        }

        private void recalcTimingButton_Click(object sender, EventArgs e)
        {
            calculateTiming();
        }

        private void FourierWindowCombobox_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            int newFourierWindow = 1024;
            switch (fourierWindowCombobox.SelectedIndex)
            {
                case 0:
                    newFourierWindow = 256;
                    break;
                case 1:
                    newFourierWindow = 512;
                    break;
                case 2:
                    newFourierWindow = 1024;
                    break;
                case 3:
                    newFourierWindow = 2048;
                    break;
                case 4:
                    newFourierWindow = 4096;
                    break;
            }

            timingPipeline.FourierWindow = newFourierWindow;
        }

        private void evalDistanceNumeric_OnValueChanged(object sender, EventArgs e)
        {
            timingPipeline.EvalDistanceSeconds = (double)evalDistanceNumeric.Value;
        }

        private void numFrequNumeric_OnValueChanged(object sender, EventArgs e)
        {
            timingPipeline.NumFrequencyBands = (int)numFreqBandsNumeric.Value;
        }

        private void AddAllFrequ_OnCheckChanded(object sender, EventArgs e)
        {
            timingPipeline.AddAllFrequenciesAtTheEnd = addAllFreqCheckbox.Checked;
        }

        private void leftChannelLeckbox_CheckedChanged(object sender, EventArgs e)
        {
            timingPipeline.LeftChannel = leftChannelCheckbox.Checked;
        }

        private void rightChannelCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            timingPipeline.RightChannel = rightChannelCheckbox.Checked;
        }

        private void binaryPeakCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            timingPipeline.BinaryPeaks = binaryPeakCheckbox.Checked;
        }

        private void strideNumeric_ValueChanged(object sender, EventArgs e)
        {
            timingPipeline.Stride = (double)strideNumeric.Value;
        }

        private void differenceFunctionCombobox_SelectedIndexChanged(object sender, EventArgs e)
        {
            FourierDifferenceType newDifferenceFunction = FourierDifferenceType.SumSquares;

            switch (differenceFunctionCombobox.SelectedIndex)
            {
                case 0:
                    newDifferenceFunction = FourierDifferenceType.SumSquares;
                    break;
                case 1:
                    newDifferenceFunction = FourierDifferenceType.MaxSample;
                    break;
            }

            timingPipeline.DifferenceFunction = newDifferenceFunction;
        }

        private void localizedTimingCheckbox_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
