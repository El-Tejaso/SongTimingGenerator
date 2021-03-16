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

        public bool ForceIndividualView {
            get; set;
        }

        public void ShowTimingPoints(TimingPointList timingPointsIn)
        {
            timingPoints = timingPointsIn;
            this.Invalidate();
        }

        Font textFont;
        StringFormat format;
        Pen drawingPen;
        SolidBrush textBrush;

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

            drawingPen = new Pen(Color.Red, 3);
            textBrush = new SolidBrush(Color.Red);
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
                return audioData.SampleToSeconds(audioData.CurrentSample);
            }
            set {
                if (audioData == null) return;
                int pos = audioData.ToSample(value);
                audioData.CurrentSample = pos;
                this.Invalidate();
            }
        }

        /// <summary>
        /// The length of the visible window in samples
        /// </summary>
        public int WindowLength {
            get {
                if (audioData == null) return 0;

                return (int)(audioData.ToSample(WindowLengthSeconds));
            }
            set {
                if (audioData == null) return;

                WindowLengthSeconds = audioData.SampleToSeconds(value);
            }
        }

        /// <summary>
        /// WindowLength, but in seconds instead of samples
        /// </summary>
        public double WindowLengthSeconds {
            get => (this.SecondsPerPixel * this.ClientRectangle.Width);
            set {
                this.SecondsPerPixel = value / ClientRectangle.Width;
            }
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

        float viewportMax = 1;

        void calculateExtents() {
            if (audioData == null)
                return;

            int lower = Math.Max(0, audioData.CurrentSample - WindowLength / 2);
            int upper = Math.Min(audioData.Data.Length, audioData.CurrentSample + WindowLength / 2);

            viewportMax = Math.Max(0.00001f, FloatArrays.Max(audioData.Data.GetSlice(lower, upper), true));
        }

        void drawWaveform(PaintEventArgs e, float rectTop, float rectBottom, int channel)
        {
            if (audioData == null)
                return;

            Slice<float> data = audioData.GetChannel(channel);

            for (float x = ClientRectangle.Left; x < ClientRectangle.Right; x += 1)
            {
                int a = waveformToDataX(x);
                if (a < 0) continue;
                if (a >= audioData.Data.Length) break;

                int samplesPerPixel = audioData.ToSample(SecondsPerPixel);
                int b = a + samplesPerPixel;

                float low = data[a];
                float high = low;

                bool reachedEnd = false;
                for (int i = a+1; i < b; i++)
                {
                    if (i >= data.Length)
                    {
                        reachedEnd = true;
                        break;
                    }

                    low = Math.Min(low, data[i]);
                    high = Math.Max(high, data[i]);
                }

                e.Graphics.DrawLine(Pens.Black, x, getWaveformY(low, rectTop, rectBottom), x,getWaveformY(high,rectTop, rectBottom));

                if (reachedEnd)
                    break;
            }
        }


        float getWaveformY(float sample, float rectTop, float rectBottom)
        {
            float percent = -0.5f * QuickMafs.Clamp(sample / Math.Abs(viewportMax), -1, 1) + 0.5f;

            float y = rectTop + (rectBottom - rectTop) * percent;

            return y;
        }


        float getWaveformX(double time)
        {
            return getWaveformX(audioData.ToSample(time));
        }

        int waveformToDataX(float waveformX)
        {
            return audioData.CurrentSample - WindowLength / 2 + (int)(((waveformX / (float)ClientRectangle.Width) * (float)WindowLength));
        }

        float getWaveformX(int samples)
        {
            return ClientRectangle.X + ((samples-audioData.CurrentSample+WindowLength/2) / (float)WindowLength) * ClientRectangle.Width;
        }


        void drawIndividualSamples(PaintEventArgs e, float rectTop, float rectBottom, int channel)
        {
            if (audioData == null)
                return;

            int samplesPerPixel = audioData.ToSample(SecondsPerPixel);

            int lower = Math.Max(0, audioData.CurrentSample - WindowLength / 2);
            int upper = Math.Min(audioData.Data.Length, audioData.CurrentSample + WindowLength / 2);

            Logger.Log(lower + " " + upper);

            Slice<float> channelData = audioData.GetChannel(channel);

            for (int position = lower; position < upper; position++)
            {
                float x = getWaveformX(position);
                float y = getWaveformY(channelData[position], rectTop, rectBottom);
                e.Graphics.DrawLine(Pens.Black, x, getWaveformY(0, rectTop, rectBottom), x, y);

                if (position > lower)
                {
                    e.Graphics.DrawLine(Pens.Black, getWaveformX(position-1), getWaveformY(channelData[position-1], rectTop, rectBottom), x, y);
                }
            }

            Slice<float> range = channelData.GetSlice(lower, upper);
            float mean = FloatArrays.Average(range, false);
			float meanAbs = FloatArrays.Average(range, true);
            float stdev = FloatArrays.StdDev(range);

            //e.Graphics.DrawString("Av = " + mean.ToString("0.0000"), textFont, Brushes.Black, new PointF(ClientRectangle.Left, ClientRectangle.Top + 100));
            //e.Graphics.DrawString("STDev = " + stdev.ToString("0.0000"), textFont, Brushes.Black, new PointF(ClientRectangle.Left, ClientRectangle.Top + 140));
            float meanY = getWaveformY(mean, rectTop, rectBottom);
            
            e.Graphics.DrawLine(Pens.Orange, ClientRectangle.Left, meanY, ClientRectangle.Right, meanY);
			
			for(int i = 1; i <= 6; i++){
				float stdevY = getWaveformY(mean+(float)i*stdev,rectTop, rectBottom);
                if (stdevY < rectTop+10) break;
                if (stdevY > rectBottom-10) break;

                e.Graphics.DrawLine(Pens.Aqua, ClientRectangle.Left, stdevY, ClientRectangle.Right, stdevY);
				e.Graphics.DrawString(i.ToString(), textFont, Brushes.Aqua, new PointF(ClientRectangle.X+ClientRectangle.Width/2 - 40, stdevY));
			}


			for(int i = 1; i <= 6; i++){
				float stdevY = getWaveformY(mean+(float)i*meanAbs, rectTop, rectBottom);
                if (stdevY < rectTop + 10) break;
                if (stdevY > rectBottom - 10) break;

                e.Graphics.DrawLine(Pens.Blue, ClientRectangle.Left, stdevY, ClientRectangle.Right, stdevY);
				e.Graphics.DrawString(i.ToString(), textFont, Brushes.Blue, new PointF(ClientRectangle.X+ClientRectangle.Width/2 - 60, stdevY));
			}
        }


        void drawAxes(PaintEventArgs e, float rectTop, float rectBottom)
        {

            int mid = ClientRectangle.X + ClientRectangle.Width / 2;
            e.Graphics.DrawLine(Pens.Yellow, mid, ClientRectangle.Top, mid, ClientRectangle.Bottom);

            e.Graphics.DrawString(viewportMax.ToString("0.00"), textFont, Brushes.LimeGreen, ClientRectangle.Left, rectTop);
            e.Graphics.DrawString(viewportMax.ToString("0.00"), textFont, Brushes.LimeGreen, ClientRectangle.Left, rectBottom - 20);
        }


        void drawTimingPoints(PaintEventArgs e)
        {
            if ((timingPoints != null) && (timingPoints.List.Count > 0))
            {
                double lowerBound = audioData.CurrentSampleSeconds - WindowLengthSeconds / 2.0;
                double upperBound = audioData.CurrentSampleSeconds + WindowLengthSeconds / 2.0;
                int startIndex = timingPoints.First(lowerBound);

                double prevTime = timingPoints.List[Math.Max(startIndex - 1, 0)].OffsetSeconds;

                for (int i = startIndex; i < timingPoints.List.Count; i++)
                {
                    TimingPoint tp = timingPoints.List[i];

                    if (tp.OffsetSeconds > upperBound)
                        break;

                    float x = getWaveformX(tp.OffsetSeconds);

                    drawingPen.Color = tp.Color;
                    textBrush.Color = tp.Color;

                    e.Graphics.DrawLine(drawingPen, x, ClientRectangle.Top, x, ClientRectangle.Bottom - 60);

                    String desc = "[" + tp.BPM.ToString("0.00") + "," + tp.OffsetSeconds.ToString("0.00") + "]" +
                            "(+ " + (tp.OffsetSeconds - prevTime).ToString("0.000") + "s)";

                    e.Graphics.DrawString(desc, textFont, textBrush, new PointF(x, ClientRectangle.Bottom - 20), format);

					e.Graphics.DrawString("W:" + tp.Weight.ToString("0.000"), textFont, Brushes.Red, new PointF(x, ClientRectangle.Bottom - 40), format);

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

            int samplesPerPixel = audioData.ToSample(SecondsPerPixel);

            for(int i = 0; i < audioData.Channels; i++)
            {
                float height = ClientRectangle.Height - 80;
                float top = ClientRectangle.Top + i * height / (float)audioData.Channels;
                float bottom = ClientRectangle.Top + (i+1) * height / (float)audioData.Channels;

                if (ForceIndividualView)
                {
                    drawIndividualSamples(e, top, bottom, i);
                }
                else
                {
                    if (samplesPerPixel > 1)
                    {
                        drawWaveform(e, top, bottom, i);
                    }
                    else
                    {
                        drawIndividualSamples(e, top, bottom, i);
                    }
                }

                drawAxes(e, top, bottom);
            }


            drawTimingPoints(e);

            e.Graphics.DrawString("" + audioData.CurrentSampleSeconds.ToString("0.000") + "s", textFont, Brushes.Blue, new PointF(ClientRectangle.X + ClientRectangle.Width / 2, ClientRectangle.Top));

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
