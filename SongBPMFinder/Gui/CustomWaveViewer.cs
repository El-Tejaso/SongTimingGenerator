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
        Pen drawingPen;

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

            drawingPen = new Pen(Color.Red);
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

        float viewportMin = -1;
        float viewportMax = 1;

        void calculateExtents() {
            if (audioData == null)
                return;

            //actually need to start at -1 and 1
            viewportMax = 0.001f; 
            viewportMin = -viewportMax;


            long lower = Math.Max(0, audioData.Position - WindowLength / 2);
            long upper = Math.Min(audioData.Data.Length, audioData.Position + WindowLength / 2);
            for (long position =  lower; position < upper; position++)
            {
                viewportMin = Math.Min(viewportMin, audioData.Data[position]);
                viewportMax = Math.Max(viewportMax, audioData.Data[position]);
            }
        }

        void drawWaveform(PaintEventArgs e)
        {
            if (audioData == null)
                return;

            for (float x = ClientRectangle.Left; x < ClientRectangle.Right; x += 1)
            {
                long a = waveformToDataX(x);
                if (a < 0) continue;
                if (a >= audioData.Data.Length) break;

                int samplesPerPixel = audioData.ToSamples(SecondsPerPixel);
                long b = a + samplesPerPixel;

                float low = audioData.Data[a];
                float high = low;

                bool reachedEnd = false;
                for (long i = a+1; i < b; i++)
                {
                    if (i >= audioData.Data.Length)
                    {
                        reachedEnd = true;
                        break;
                    }

                    low = Math.Min(low, audioData.Data[i]);
                    high = Math.Max(high, audioData.Data[i]);
                }

                e.Graphics.DrawLine(Pens.Black, x, getWaveformY(low), x,getWaveformY(high));

                if (reachedEnd)
                    break;
            }
        }


        float getWaveformY(float sample)
        {
            if (sample < 0)
            {
                float lowPercent = -0.5f * QuickMafs.Clamp(sample / Math.Abs(viewportMax), -1, 1) + 0.5f;
                return ClientRectangle.Top + ClientRectangle.Height * lowPercent - 1;
            } else
            {
                float highPercent = -0.5f * QuickMafs.Clamp(sample / Math.Abs(viewportMax), -1, 1) + 0.5f;
                return ClientRectangle.Top + ClientRectangle.Height * highPercent;
            }
        }


        float getWaveformX(double time)
        {
            return getWaveformX(audioData.ToSamples(time));
        }

        long waveformToDataX(float waveformX)
        {
            return audioData.Position - WindowLength / 2 + (long)(((waveformX / (float)ClientRectangle.Width) * (float)WindowLength));
        }

        float getWaveformX(long samples)
        {
            return ClientRectangle.X + ((samples-audioData.Position+WindowLength/2) / (float)WindowLength) * ClientRectangle.Width;
        }

        void drawIndividualSamples(PaintEventArgs e)
        {
            if (audioData == null)
                return;
            int samplesPerPixel = audioData.ToSamples(SecondsPerPixel);

            if (samplesPerPixel > 1) return;

            long lower = Math.Max(0, audioData.Position - WindowLength / 2);
            long upper = Math.Min(audioData.Data.Length, audioData.Position + WindowLength / 2);


            for (long position = lower; position < upper; position++)
            {
                float x = getWaveformX(position);
                float y = getWaveformY(audioData.Data[position]);
                e.Graphics.DrawLine(Pens.Black, x, getWaveformY(0), x, y);

                if (position > lower)
                {
                    e.Graphics.DrawLine(Pens.Black, getWaveformX(position-1), getWaveformY(audioData.Data[position-1]), x, y);
                }
            }
        }


        void drawAxes(PaintEventArgs e)
        {

            int mid = ClientRectangle.X + ClientRectangle.Width / 2;
            e.Graphics.DrawLine(Pens.Yellow, mid, ClientRectangle.Top, mid, ClientRectangle.Bottom);

            e.Graphics.DrawString(viewportMax.ToString("0.00"), textFont, Brushes.LimeGreen, ClientRectangle.Left, ClientRectangle.Top);
            e.Graphics.DrawString(viewportMax.ToString("0.00"), textFont, Brushes.LimeGreen, ClientRectangle.Left, ClientRectangle.Bottom - 20);
        }


        void drawTimingPoints(PaintEventArgs e)
        {
            if ((timingPoints != null) && (timingPoints.List.Count > 0))
            {
                double lowerBound = audioData.PositionSeconds - WindowLengthSeconds / 2.0;
                double upperBound = audioData.PositionSeconds + WindowLengthSeconds / 2.0;
                int startIndex = timingPoints.First(lowerBound);

                double prevTime = timingPoints.List[Math.Max(startIndex - 1, 0)].OffsetSeconds;

                for (int i = startIndex; i < timingPoints.List.Count; i++)
                {
                    TimingPoint tp = timingPoints.List[i];

                    if (tp.OffsetSeconds > upperBound)
                        break;

                    float x = getWaveformX(tp.OffsetSeconds);

                    drawingPen.Color = tp.Color;
                    e.Graphics.DrawLine(drawingPen, x, ClientRectangle.Top, x, ClientRectangle.Bottom - 40);

                    String desc = "[" + tp.BPM.ToString("0.00") + "," + tp.OffsetSeconds.ToString("0.00") + "]" +
                            "(+ " + (tp.OffsetSeconds - prevTime).ToString("0.000") + "s)";

                    e.Graphics.DrawString(desc, textFont, Brushes.Red, new PointF(x, ClientRectangle.Bottom - 20), format);

                    prevTime = tp.OffsetSeconds;
                }
            }
        }

        /// <summary>
        /// <see cref="Control.OnPaint"/>
        /// </summary>
        protected override void OnPaint(PaintEventArgs e)
        {
            if (audioData == null) return;

            calculateExtents();

            int samplesPerPixel = audioData.ToSamples(SecondsPerPixel);

            if (samplesPerPixel > 1)
            {
                drawWaveform(e);
            } else
            {
                drawIndividualSamples(e);
            }


            drawTimingPoints(e);

            drawAxes(e);

            e.Graphics.DrawString("" + audioData.PositionSeconds.ToString("0.000") + "s", textFont, Brushes.Blue, new PointF(ClientRectangle.X + ClientRectangle.Width / 2, ClientRectangle.Top));

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
