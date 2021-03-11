using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SongBPMFinder.Util;
using SongBPMFinder.Audio;
using SongBPMFinder.Audio.Timing;
using System.Diagnostics;
using SongBPMFinder.Gui;

namespace SongBPMFinder
{
    public partial class Form1 : Form
    {
        AudioData currentAudioFile = null;
        AudioDataStream audioStream;
        AudioPlayer player = new AudioPlayer();
        TimingPointList tpList;

        MethodInvoker updateSBPos;

        //m
        Playback currentPlayback = Playback.Realtime;

        #region shitCode
        //Delete these in production
        static Form1 singletoninstance;
        public static Form1 Instance => singletoninstance;

        public CustomWaveViewer Viewer;

		public bool IsTesting = false;
        #endregion

        public Form1()
        {
            InitializeComponent();

            #region shitCode

            Viewer = this.audioViewer;

            if (singletoninstance == null)
            {
                singletoninstance = this;
            }

            #endregion

            waveformTabs.MouseWheel += audioViewer_OnScroll;

            songPositionChangedInterrupt.Stop();
            songPositionChangedInterrupt.Interval = 3;

            Logger.SetOutput(textOutput);
            audioViewer.SecondsPerPixel = 0.1;

            plotWaveViewer.Data = new AudioData(new float[0], 1, 1);

            updateSBPos = new MethodInvoker(delegate ()
            {
                if (this.IsDisposed)
                    return;
                playbackScrollbar.Value = currentAudioFile.Position + playbackScrollbar.Minimum;
                audioViewer.Invalidate();
            });


            //TESTING

            loadFile("D:\\Archives\\Music\\Test\\Test1.mp3");
			//loadFile("D:\\Archives\\Music\\Test\\Test1-5.mp3");
			//loadFile("D:\\Archives\\Music\\Test\\Test2.mp3");
			calculateTiming();            
        }

        //Testing
		public void Plot(Slice<float> data){
            plotWaveViewer.Data = new AudioData(data.GetArray(), currentAudioFile.SampleRate, currentAudioFile.Channels);
			plotWaveViewer.WindowLength = data.Length;
			plotWaveViewer.Data.Position = data.Length/2;
        }

		//Testing
		public void AddLines(List<TimingPoint> timingPoints){
			plotWaveViewer.ShowTimingPoints(new TimingPointList(timingPoints));
		}

        private void clearOutputButton_Click(object sender, EventArgs e)
        {
            Logger.ClearOutput();
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
            if (currentAudioFile == null) return;

            int dir = e.Delta > 1 ? 1 : -1;

            if (Control.ModifierKeys == Keys.Control)
            {
                audioViewer.SecondsPerPixel *= Math.Pow(2, -dir);

                if (audioViewer.SecondsPerPixel < 0.000001f)
                    audioViewer.SecondsPerPixel = 0.000001f;

                if (audioViewer.SecondsPerPixel > currentAudioFile.Duration / audioViewer.Width)
                    audioViewer.SecondsPerPixel = currentAudioFile.Duration / audioViewer.Width;

            }
            else
            {
                if(ModifierKeys == Keys.Shift)
                {
                    currentAudioFile.Position -= dir * audioViewer.WindowLength / 100;
                } 
                else
                {
                    currentAudioFile.Position -= dir * audioViewer.WindowLength / 10;
                }

                audioViewer.Invalidate();

                //TESTING
				if(IsTesting){
                	calculateTiming();
				}
            }


            UpdateScrollExtents();
        }

        private void songPositionChangedInterrupt_Tick(object sender, EventArgs e)
        {
            updateSBPos();
        }

        private void playbackScrollbar_Scroll(object sender, ScrollEventArgs e)
        {
            if (currentAudioFile == null) return;
            UpdateViewerScroll();
        }

        private void calcTimingButton_Click(object sender, EventArgs e)
        {
            calculateTiming();
        }

        private void copyTimingButton_Click(object sender, EventArgs e)
        {
            copyTimingToClipboard();
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


        void loadFile(string filename)
        {
            if (currentAudioFile != null)
            {
                audioViewer.Data = null;
                currentAudioFile = null;
                audioStream = null;
                GC.Collect();
            }

            currentAudioFile = new AudioData(filename);
            audioViewer.Data = currentAudioFile;

            audioStream = new AudioDataStream(currentAudioFile);
            player.SetAudio(audioStream);

            calcTimingButton.Enabled = true;
            copyTimingButton.Enabled = false;
            setActiveSpeedButton(buttonSpeed1x);

            tpList = null;

            UpdateScrollExtents();
            UpdateViewerScroll();
        }

      
        void UpdateViewerScroll()
        {
            currentAudioFile.Position = playbackScrollbar.Value - playbackScrollbar.Minimum;
            audioViewer.Invalidate();
        }

        void UpdateScrollExtents()
        {
            int windowLength = audioViewer.WindowLength;
            playbackScrollbar.Minimum = -windowLength / 2;
            playbackScrollbar.Maximum = Math.Max(0, currentAudioFile.Data.Length - windowLength / 2);
            playbackScrollbar.Value = playbackScrollbar.Minimum + currentAudioFile.Position;
        }

        void playPause()
        {
            if (currentAudioFile == null) return;

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

        void calculateTiming()
        {
            var watch = Stopwatch.StartNew();
            tpList = Timing.GenerateMultiBPMTiming(currentAudioFile);

            audioViewer.ShowTimingPoints(tpList);

            watch.Stop();
            Logger.Log("Calculated timing in " + (watch.ElapsedMilliseconds / 1000.0).ToString("0.000") + " Seconds");
			Logger.Log("" + tpList.List.Count + " timing points were generated.");
            copyTimingButton.Enabled = true;
        }


        void copyTimingToClipboard()
        {
            if (tpList == null)
            {
                copyTimingButton.Enabled = false;
                Logger.Log("Oops that button wasn't supposed to be enabled, sorry. Open a song, calculate the timing, then click here again");
                return;
            }

            String s = tpList.ToString(TimingPointList.TimingFormat.Osu);
            Clipboard.SetText(s);

            Logger.Log("Copied the following to the clipboard:\n\"" + s + "\n\"");
        }

        Button currentSpeedButton = null;
        Color initBackColor;

        void setActiveSpeedButton(Button b)
        {
            if(currentSpeedButton == null)
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
    }
}
