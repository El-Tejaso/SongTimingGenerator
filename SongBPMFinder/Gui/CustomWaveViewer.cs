using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using NAudio.Wave;
using SongBPMFinder.Audio;
using SongBPMFinder.Audio.Timing;
using SongBPMFinder.Util;

namespace SongBPMFinder.Gui
{
    /// <summary>
    /// Control for viewing waveforms. Initially Naudio.Gui.WaveViewer from the NAudio Library, it has been adapted to suit
	/// my own neeeds
    /// </summary>
    public class CustomWaveViewer : System.Windows.Forms.UserControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

        private AudioData audioData;
        private double secondsPerPixel;
        private TimingPointList timingPoints;

        public void ShowTimingPoints(TimingPointList timingPointsIn)
        {
            timingPoints = timingPointsIn;
            this.Invalidate();
        }

        Font textFont;
        StringFormat format;

        /// <summary>
        /// Creates a new WaveViewer control
        /// </summary>
        public CustomWaveViewer()
        {
            // This call is required by the Windows.Forms Form Designer.
            InitializeComponent();
            this.ResizeRedraw = true;
            this.DoubleBuffered = true;

            textFont = new Font(SystemFonts.DefaultFont.FontFamily, 12.0f, FontStyle.Regular);
            format = new StringFormat();
            format.Alignment = StringAlignment.Center;
        }

        /// <summary>
        /// sets the associated waveData
        /// </summary>
        public AudioData Data {
            get {
                return audioData;
            }
            set {
                audioData = value;
                this.Invalidate();
            }
        }

        /// <summary>
        /// The zoom level, in seconds
        /// </summary>
        public double SecondsPerPixel {
            get {
                return secondsPerPixel;
            }
            set {
                secondsPerPixel = value;
                this.Invalidate();
            }
        }

        /// <summary>
        /// Start time (in seconds)
        /// </summary>
        public double StartTime{
            get {
                if (audioData == null) return 0.0;
                return audioData.ToSeconds(audioData.Position);
            }
            set {
                if (audioData == null) return;
                audioData.Position = audioData.ToSamples(value * audioData.SampleRate);
                this.Invalidate();
            }
        }

        /// <summary>
        /// The length of the visible window in samples
        /// </summary>
        public int WindowLength {
            get => (int)(audioData.ToSamples(WindowLengthSeconds));
        }

        /// <summary>
        /// WindowLength, but in seconds instead of samples
        /// </summary>
        public double WindowLengthSeconds {
            get => (this.SecondsPerPixel * this.ClientRectangle.Width);
        }


        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// <see cref="Control.OnPaint"/>
        /// </summary>
        protected override void OnPaint(PaintEventArgs e)
        {
            if (audioData!= null)
            {
                long position = audioData.Position -WindowLength/2;
				float minValue = -1;
				float maxValue = 1;
				
                for (float x = e.ClipRectangle.X; x < e.ClipRectangle.Right; x += 1)
                {
                    int samplesPerPixel = audioData.ToSamples(SecondsPerPixel);
                    if (position < 0)
                    {
                        position += samplesPerPixel;
                        continue;
                    }

                    bool reachedEnd = false;
                    for(long i = 0; i < samplesPerPixel; i++)
                    {
                        long index = position + i;
                        if (index >= audioData.Data.Length)
                        {
                            reachedEnd = true;
                            break;
                        }

                        minValue = Math.Min(minValue, audioData.Data[index]);
                        maxValue = Math.Max(maxValue, audioData.Data[index]);
                    }
                    position += samplesPerPixel;
                    if (reachedEnd)
                        break;
                }

                position = audioData.Position -WindowLength/2;

                for (float x = e.ClipRectangle.X; x < e.ClipRectangle.Right; x += 1)
                {
                    int samplesPerPixel = audioData.ToSamples(SecondsPerPixel);
                    if (position < 0)
                    {
                        position += samplesPerPixel;
                        continue;
                    }

                    float low = 1;
                    float high = -1;

                    bool reachedEnd = false;
                    for(long i = 0; i < samplesPerPixel; i++)
                    {
                        long index = position + i;
                        if (index >= audioData.Data.Length)
                        {
                            reachedEnd = true;
                            break;
                        }

                        low = Math.Min(low, audioData.Data[index]);
                        high= Math.Max(high, audioData.Data[index]);
                    }
                    position += samplesPerPixel;

                    float lowPercent = -0.5f * (low/Math.Abs(minValue)) + 0.5f;
                    float highPercent = -0.5f * (high/Math.Abs(maxValue)) + 0.5f;
                    //float lowPercent = low;
                    //float highPercent = high;
                    e.Graphics.DrawLine(Pens.Black, x, e.ClipRectangle.Y + this.Height * lowPercent - 1, x, e.ClipRectangle.Y + this.Height * highPercent);

                    if (reachedEnd)
                        break;
                }

                int mid = ClientRectangle.X + ClientRectangle.Width / 2;
                e.Graphics.DrawLine(Pens.Yellow, mid, ClientRectangle.Top, mid, ClientRectangle.Bottom);

                e.Graphics.DrawString(maxValue.ToString("0.00"), textFont, Brushes.LimeGreen, ClientRectangle.Left, ClientRectangle.Top);
                e.Graphics.DrawString(minValue.ToString("0.00"), textFont, Brushes.LimeGreen, ClientRectangle.Left, ClientRectangle.Bottom - 20);
            }


            if (timingPoints!= null)
            {
                double lowerBound = audioData.PositionSeconds - WindowLengthSeconds / 2.0;
                double upperBound = audioData.PositionSeconds + WindowLengthSeconds / 2.0;
                int startIndex = timingPoints.First(lowerBound);
                
                for(int i = startIndex; i< timingPoints.List.Count; i++)
                {
                    TimingPoint tp = timingPoints.List[i];
                    double relativeTime = tp.OffsetSeconds;

                    if (relativeTime > upperBound)
                        break;

                    //xD Lmao
                    double xD = ClientRectangle.X + ClientRectangle.Width * ((relativeTime - lowerBound)/WindowLengthSeconds);
                    float x = (float)xD;

                    e.Graphics.DrawLine(Pens.Red, x, ClientRectangle.Top+40, x, ClientRectangle.Bottom-40);
                    e.Graphics.DrawString("[" + tp.BPM.ToString("0.00") + "," + tp.OffsetSeconds.ToString("0.00") + "]", textFont, Brushes.Red, new PointF(x, ClientRectangle.Bottom - 20), format);
                }
            }

            e.Graphics.DrawString("" + audioData.PositionSeconds, textFont, Brushes.Blue, new PointF(ClientRectangle.X + ClientRectangle.Width / 2, ClientRectangle.Top));

            base.OnPaint(e);
        }

        #region Component Designer generated code
        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // WaveViewer
            // 
            this.Name = "WaveViewer";
            this.ResumeLayout(false);

        }
        #endregion
    }
}
