using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace SongBPMFinder
{
    public partial class Form1 : Form
    {
        AudioData currentAudioFile = null;
        AudioData currentAudioFileCopy = null;

        AudioDataStream audioStream;
        AudioPlayer player = new AudioPlayer();

        MethodInvoker updateSBPos;

        TimingPointList currentTimingResult;

        //m
        Playback currentPlayback = Playback.Realtime;

        public bool IsTesting = true;

        public Form1()
        {
            InitializeComponent();

            waveformTabs.MouseWheel += audioViewer_OnScroll;

            songPositionChangedInterrupt.Stop();
            songPositionChangedInterrupt.Interval = 3;

            Logger.SetOutput(new RichTextBoxLogger(textOutput));
            audioViewer.SecondsPerPixel = 0.1;


            updateSBPos = new MethodInvoker(delegate () {
                if (this.IsDisposed)
                    return;
                playbackScrollbar.Value = currentAudioFile.CurrentSample + playbackScrollbar.Minimum;
                audioViewer.Invalidate();
            });


            //TESTING

            loadFile("D:\\Archives\\Music\\Test\\Test0-5.mp3");
            //loadFile("D:\\Archives\\Music\\Test\\Test1.mp3");
            //loadFile("D:\\Archives\\Music\\Test\\Early Summer.mp3");
            //loadFile("D:\\Archives\\Music\\Test\\Test1-5.mp3");
            //loadFile("D:\\Archives\\Music\\Test\\Test2.mp3");

            calculateTiming();
        }


        private CustomWaveViewer GetCurrentViewer()
        {
            if (freezeView.Checked)
            {
                if (waveformTabs.SelectedIndex == 0)
                    return null;

                return getViewer(waveformTabs.SelectedIndex - 1);
            }


            return audioViewer;
        }

        CustomWaveViewer getViewer(int graph)
        {
            switch (graph)
            {
                case 1:
                    return plotWaveViewer2;
                case 2:
                    return plotWaveViewer3;
                case 3:
                    return plotWaveViewer4;
                case 4:
                    return plotWaveViewer5;
                default:
                    return plotWaveViewer;
            }
        }

        TabPage getPage(int graph)
        {
            switch (graph)
            {
                case 1:
                    return testWaveformTab2;
                case 2:
                    return testWaveformTab3;
                case 3:
                    return testWaveformTab4;
                case 4:
                    return testWaveformTab5;
                default:
                    return testWaveformTab;
            }
        }

        //Testing
        public void Plot(string name, AudioSlice data, int graph)
        {
            CustomWaveViewer viewer = getViewer(graph);
            TabPage page = getPage(graph);
            data = data.DeepCopy();

            page.Text = name;

            viewer.Data = new AudioData(new AudioSlice[] { data }, currentAudioFile.SampleRate);

            viewer.WindowLengthSamples = data.Length;
            viewer.Data.CurrentSample = data.Length / 2;
        }

        //Testing
        public void AddLines(SortedList<TimingPoint> timingPoints, int graph)
        {
            CustomWaveViewer viewer = getViewer(graph);

            viewer.ShowTimingPoints(new TimingPointList(timingPoints));
        }

        private void clearOutputButton_Click(object sender, EventArgs e)
        {
            Logger.Clear();
        }
        private void playPauseButton_Click(object sender, EventArgs e)
        {
            playPause();
        }

        private void openButton_Click(object sender, EventArgs e)
        {
            openFileDialogue.Filter = "Audio Files(*.mp3, *.mp4)|*.mp3;*.mp4";
            openFileDialogue.Title = "Open an audio file for timing";

            DialogResult res = openFileDialogue.ShowDialog();

            if (res == DialogResult.OK)
            {
                loadFile(openFileDialogue.FileName);
            }
        }

        //Implements zooming in and out, and scrolling
        private void audioViewer_OnScroll(object sender, MouseEventArgs e)
        {
            if (currentAudioFile == null)
                return;

            int dir = e.Delta > 1 ? 1 : -1;

            if (Control.ModifierKeys == Keys.Control)
            {
                Zoom(dir);
            }
            else
            {
                Scroll(dir);
            }
        }

        private void Scroll(int dir)
        {
            var viewer = GetCurrentViewer();
            if (viewer == null)
                return;

            if (ModifierKeys == Keys.Shift)
            {
                viewer.ScrollAudio(dir * 0.1f);
            }
            else
            {
                viewer.ScrollAudio(dir);
            }

            UpdateScrollExtents();
        }

        private void Zoom(int dir)
        {
            var viewer = GetCurrentViewer();
            if (viewer == null)
                return;

            viewer.Zoom(dir, 2.0f);
            UpdateScrollExtents();
        }

        private void songPositionChangedInterrupt_Tick(object sender, EventArgs e)
        {
            updateSBPos();
        }

        private void playbackScrollbar_Scroll(object sender, ScrollEventArgs e)
        {
            if (currentAudioFile == null)
                return;
            UpdateViewerScroll();
        }

        private void calcTimingButton_Click(object sender, EventArgs e)
        {
            calculateTiming();
        }

        void calculateTiming()
        {
            var watch = Stopwatch.StartNew();

            IBeatDetector beatDetector = new BeatDetector();
            ITimingGenerator timingGenerator = new TestTimingGenerator();

            SortedList<Beat> beats = beatDetector.GetEveryBeat(currentAudioFile.GetChannel(0));
            currentTimingResult = timingGenerator.GenerateTiming(beats);


            audioViewer.ShowTimingPoints(currentTimingResult);
            watch.Stop();

            Logger.Log("Calculated timing in " + (watch.ElapsedMilliseconds / 1000.0).ToString("0.000") + " Seconds");
            Logger.Log("" + currentTimingResult.Count + " timing points were generated.");
            copyTimingButton.Enabled = true;
        }


        private void copyTimingButton_Click(object sender, EventArgs e)
        {
            copyTimingToClipboard();
        }

        void copyTimingToClipboard()
        {
            if (currentTimingResult == null)
            {
                copyTimingButton.Enabled = false;
                Logger.Log("Oops that button wasn't supposed to be enabled, sorry. Open a song, calculate the timing, then click here again");
                return;
            }

            OsuTimingPointFormatter formatter = new OsuTimingPointFormatter();

            string osuFormattedTimingPoints = formatter.FormatTiming(currentTimingResult);
            Clipboard.SetText(osuFormattedTimingPoints);

            Logger.Log("Copied the following to the clipboard:\n\"" + osuFormattedTimingPoints + "\n\"");
        }


        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space)
            {
                playPauseButton.Focus();
                playPause();
                e.Handled = true;
            }
        }

        void setCurrentAudio(AudioData audioData)
        {
            if (IsTesting)
            {
                if (audioData != currentAudioFile)
                {
                    currentAudioFileCopy = audioData.DeepCopy();
                }
            }

            currentAudioFile = audioData;
            audioViewer.Data = audioData;

            audioStream = new AudioDataStream(currentAudioFile);
            player.SetAudio(audioStream);

            setActiveSpeedButton(buttonSpeed1x);

            UpdateScrollExtents();
            UpdateViewerScroll();
        }

        void loadFile(string filename)
        {
            if (currentAudioFile != null)
            {
                audioViewer.Data = null;

                currentAudioFile = null;

                if (IsTesting)
                {
                    currentAudioFileCopy = null;
                }

                audioStream = null;
                GC.Collect();
            }

            setCurrentAudio(AudioData.FromFile(filename));

            calcTimingButton.Enabled = true;
            copyTimingButton.Enabled = false;
        }


        void UpdateViewerScroll()
        {
            currentAudioFile.CurrentSample = playbackScrollbar.Value - playbackScrollbar.Minimum;
            audioViewer.Invalidate();
        }

        void UpdateScrollExtents()
        {
            int windowLength = audioViewer.WindowLengthSamples;
            playbackScrollbar.Minimum = -windowLength / 2;
            playbackScrollbar.Maximum = Math.Max(0, currentAudioFile.Length - windowLength / 2);
            playbackScrollbar.Value = playbackScrollbar.Minimum + currentAudioFile.CurrentSample;
        }

        void playPause()
        {
            if (currentAudioFile == null)
                return;

            if (player.IsPlaying)
            {
                player.Pause();
                playPauseButton.Text = "4";
                songPositionChangedInterrupt.Stop();
            }
            else
            {
                player.Play();
                playPauseButton.Text = ";";
                songPositionChangedInterrupt.Start();
            }
        }

        Button currentSpeedButton = null;
        Color initBackColor;

        void setActiveSpeedButton(Button b)
        {
            if (currentSpeedButton == null)
            {
                initBackColor = b.BackColor;
            }
            else
            {
                currentSpeedButton.BackColor = initBackColor;
            }

            currentSpeedButton = b;
            b.BackColor = Color.Aqua;
        }

        private void buttonSpeed1x_Click(object sender, EventArgs e)
        {
            audioStream.Playback = Playback.Realtime;
            setActiveSpeedButton(buttonSpeed1x);
        }

        private void buttonSpeed075x_Click(object sender, EventArgs e)
        {
            audioStream.Playback = Playback.ThreeFourths;
            setActiveSpeedButton(buttonSpeed075x);

        }

        private void buttonSpeed050x_Click(object sender, EventArgs e)
        {
            audioStream.Playback = Playback.Halftime;
            setActiveSpeedButton(buttonSpeed050x);
        }

        private void buttonSpeed025x_Click(object sender, EventArgs e)
        {
            audioStream.Playback = Playback.Quartertime;
            setActiveSpeedButton(buttonSpeed025x);
        }

        private void testButton_Click(object sender, EventArgs e)
        {

        }

        private void freezeView_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
