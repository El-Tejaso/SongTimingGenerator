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

        public Form1()
        {
            InitializeComponent();
            Logger.SetOutput(new RichTextBoxLogger(textOutput));
            audioPlaybackSystem = new AudioPlaybackSystem();

            audioViewer.LinkPlaybackSystem(audioPlaybackSystem);

            audioPlaybackSystem.OnNewSongLoad += AudioPlaybackSystem_OnNewSongLoad;

            audioPlaybackSystem.LoadFile("D:\\Archives\\Music\\Test\\Test0-5.mp3");
        }

        private void AudioPlaybackSystem_OnNewSongLoad()
        {
            calcTimingButton.Enabled = true;
            
        }

        CustomWaveViewer getViewer(int graph)
        {
            switch (graph)
            {
                case 1:
                    return debugPlot1;
                case 2:
                    return debugPlot2;
                case 3:
                    return debugPlot3;
                case 4:
                    return debugPlot4;
                default:
                    return debugPlot5;
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

        private void clearOutputButton_Click(object sender, EventArgs e)
        {
            Logger.Clear();
        }
        private void playPauseButton_Click(object sender, EventArgs e)
        {
            playPause();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space)
            {
                playPause();
                e.Handled = true;
            }
        }

        void playPause()
        {
            audioPlaybackSystem.PlayPause();
        }

        private void openButton_Click(object sender, EventArgs e)
        {
            audioPlaybackSystem.ShowOpenFilePrompt();
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
            audioPlaybackSystem.Playback = Playback.Realtime;
            setActiveSpeedButton(buttonSpeed1x);
        }

        private void buttonSpeed075x_Click(object sender, EventArgs e)
        {
            audioPlaybackSystem.Playback = Playback.ThreeFourths;
            setActiveSpeedButton(buttonSpeed075x);

        }

        private void buttonSpeed050x_Click(object sender, EventArgs e)
        {
            audioPlaybackSystem.Playback = Playback.Halftime;
            setActiveSpeedButton(buttonSpeed050x);
        }

        private void buttonSpeed025x_Click(object sender, EventArgs e)
        {
            audioPlaybackSystem.Playback = Playback.Quartertime;
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
