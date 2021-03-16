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
        AudioData currentAudioFileCopy = null;

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

		public bool IsTesting = true;
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


            updateSBPos = new MethodInvoker(delegate ()
            {
                if (this.IsDisposed)
                    return;
                playbackScrollbar.Value = currentAudioFile.CurrentSample + playbackScrollbar.Minimum;
                audioViewer.Invalidate();
            });


            //TESTING

            loadFile("D:\\Archives\\Music\\Test\\Test1.mp3");
			//loadFile("D:\\Archives\\Music\\Test\\Early Summer.mp3");
			//loadFile("D:\\Archives\\Music\\Test\\Test1-5.mp3");
			//loadFile("D:\\Archives\\Music\\Test\\Test2.mp3");

			calculateTiming();           
        }

        CustomWaveViewer getViewer(int graph)
        {
            switch (graph)
            {
                case 1: return plotWaveViewer2;
                case 2: return plotWaveViewer3;
                case 3: return plotWaveViewer4;
                case 4: return plotWaveViewer5;
                default: return plotWaveViewer;
            }
        }

        TabPage getPage(int graph)
        {
            switch (graph)
            {
                case 1: return testWaveformTab2;
                case 2: return testWaveformTab3;
                case 3: return testWaveformTab4;
                case 4: return testWaveformTab5;
                default: return testWaveformTab;
            }
        }

        //Testing
        public void Plot(string name, Slice<float> data, int graph){
            CustomWaveViewer viewer = getViewer(graph);
            TabPage page = getPage(graph);

            page.Text = name;
            viewer.Data = new AudioData(data, currentAudioFile.SampleRate, 1);
            viewer.WindowLength = data.Length;
            viewer.Data.CurrentSample = data.Length/2;
        }

		//Testing
		public void AddLines(List<TimingPoint> timingPoints, int graph){
            CustomWaveViewer viewer = getViewer(graph);

            viewer.ShowTimingPoints(new TimingPointList(timingPoints));
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
                audioViewer.Zoom(dir, 2.0f);
            }
            else 
            {
                if (ModifierKeys == Keys.Shift)
                {
                    audioViewer.Scroll(dir * 0.1f);
                }
                else
                {
                    audioViewer.Scroll(dir);
                }

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

			//TESTING
			if(IsTesting){
				calculateTiming();
			}
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

        void setCurrentAudio(AudioData audioData)
        {
            currentAudioFile = audioData;

            if (IsTesting)
            {
                if(audioData != currentAudioFileCopy)
                {
                    currentAudioFileCopy = new AudioData(audioData);
                } 
            }

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

            setCurrentAudio(new AudioData(filename));

            calcTimingButton.Enabled = true;
            copyTimingButton.Enabled = false;

            tpList = null;
        }

      
        void UpdateViewerScroll()
        {
            currentAudioFile.CurrentSample = playbackScrollbar.Value - playbackScrollbar.Minimum;
            audioViewer.Invalidate();
        }

        void UpdateScrollExtents()
        {
            int windowLength = audioViewer.WindowLength;
            playbackScrollbar.Minimum = -windowLength / 2;
            playbackScrollbar.Maximum = Math.Max(0, currentAudioFile.Data.Length - windowLength / 2);
            playbackScrollbar.Value = playbackScrollbar.Minimum + currentAudioFile.CurrentSample;
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
            if (IsTesting)
            {
                tpList = Timing.TestBeatFinding(currentAudioFile);
            }
            else
            {
                tpList = Timing.GenerateMultiBPMTiming(currentAudioFile);
            }
            

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

        private void testButton_Click(object sender, EventArgs e)
        {
            setCurrentAudio(currentAudioFileCopy);

            int window = (int)(currentAudioFile.SampleRate);
            window = QuickMafs.NearestPower(window, 2)*2;


            var watch = Stopwatch.StartNew();

            Slice<float> subsetL = currentAudioFile.GetChannel(0).GetSlice(currentAudioFile.CurrentSample, currentAudioFile.CurrentSample+window);
            Slice<float> imaginaryBufferL = FloatArrays.ZeroesLike(subsetL);

            Slice<float> subsetR = currentAudioFile.GetChannel(1).GetSlice(currentAudioFile.CurrentSample, currentAudioFile.CurrentSample + window);
            Slice<float> imaginaryBufferR = FloatArrays.ZeroesLike(subsetR);

            AccordFourierTransform.FFT(subsetL, imaginaryBufferL);
            AccordFourierTransform.FFT(subsetR, imaginaryBufferR);

            for(int i = 0; i < imaginaryBufferL.Length/2; i++)
            {
                subsetL[i] *= subsetL[i];

                subsetR[i] *= subsetR[i];
            }

            AccordFourierTransform.IFFT(subsetR, imaginaryBufferR);
            AccordFourierTransform.IFFT(subsetL, imaginaryBufferL);


            watch.Stop();
            Logger.Log("Did FFT and IFFT with w=" + window + " in " + watch.ElapsedMilliseconds + "ms");
        }

        private void freezeView_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
