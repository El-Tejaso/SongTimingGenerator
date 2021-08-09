using System;
using System.Drawing;
using System.Windows.Forms;

namespace SongBPMFinder
{
    /// <summary>
    /// Control for viewing waveforms. Initially Naudio.Gui.WaveViewer from the NAudio Library, it has been adapted to suit
	/// my own neeeds
    /// </summary>
    public class CustomWaveViewer : System.Windows.Forms.UserControl
    {
        #region Required Designer Variables
        private System.ComponentModel.Container components = null;
        #endregion

        private readonly Pen WAVEPEN = Pens.Black;

        private AudioData audioData;
        public AudioData Data {
            get {
                return audioData;
            }
            set {
                audioData = value;
                this.Invalidate();
            }
        }

        public double StartTimeSeconds {
            get {
                if (audioData == null)
                    return 0.0;
                return audioData.SampleToSeconds(audioData.CurrentSample);
            }
            set {
                if (audioData == null)
                    return;
                int pos = audioData.ToSample(value);
                audioData.CurrentSample = pos;
                this.Invalidate();
            }
        }

        public int WindowLengthSamples {
            get {
                if (audioData == null)
                    return 0;

                return (int)(audioData.ToSample(WindowLengthSeconds));
            }
            set {
                if (audioData == null)
                    return;

                WindowLengthSeconds = audioData.SampleToSeconds(value);
            }
        }

        public double WindowLengthSeconds {
            get => (this.SecondsPerPixel * this.ClientRectangle.Width);
            set {
                this.SecondsPerPixel = value / ClientRectangle.Width;
            }
        }

        private double secondsPerPixel;
        public double SecondsPerPixel {
            get {
                return secondsPerPixel;
            }
            set {
                secondsPerPixel = value;
                this.Invalidate();
            }
        }

        private TimingPointList timingPoints;

        float viewportMax = 1;

        public bool ForceIndividualView {
            get; set;
        }

        Font textFont;
        StringFormat format;
        Pen drawingPen;
        SolidBrush textBrush;

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


        public void Zoom(int dir, float amount)
        {
            if (audioData == null)
                return;

            SecondsPerPixel *= Math.Pow(amount, -dir);

            if (SecondsPerPixel < 0.000001f)
                SecondsPerPixel = 0.000001f;

            if (SecondsPerPixel > audioData.Duration / Width)
                SecondsPerPixel = audioData.Duration / Width;
        }


        public void ScrollAudio(float amount)
        {
            if (audioData == null)
                return;
            audioData.CurrentSample -= (int)(amount * WindowLengthSamples / 20);
            Invalidate();
        }

        public void ShowTimingPoints(TimingPointList list)
        {
            timingPoints = list;
            Invalidate();
        }

        /// <summary>
        /// <see cref="Control.OnPaint"/>
        /// </summary>
        protected override void OnPaint(PaintEventArgs e)
        {
            if (audioData == null)
                return;

            calculateExtents();

            for (int i = 0; i < audioData.Channels; i++)
            {
                drawAudioChannelWaveform(e, i);
            }

            drawTimingPoints(e);

            drawCurrentPositionText(e);

            drawMidLine(e);

            base.OnPaint(e);
        }

        void calculateExtents()
        {
            if (audioData == null)
                return;

            int lower = Math.Max(0, audioData.CurrentSample - WindowLengthSamples / 2);
            int upper = Math.Min(audioData.GetChannel(0).Length, audioData.CurrentSample + WindowLengthSamples / 2);

            viewportMax = Math.Max(0.00001f, FloatSliceUtil.Max(audioData.GetChannel(0).Slice.GetSlice(lower, upper), true));
        }


        private void drawAudioChannelWaveform(PaintEventArgs e, int i)
        {
            float height = ClientRectangle.Height - 80;
            float top = ClientRectangle.Top + i * height / (float)audioData.Channels;
            float bottom = ClientRectangle.Top + (i + 1) * height / (float)audioData.Channels;

            int samplesPerPixel = audioData.ToSample(SecondsPerPixel);
            if (ForceIndividualView || samplesPerPixel < 1)
            {
                drawIndividualSamples(e, top, bottom, i);
            }
            else
            {
                drawCondensedWaveform(e, top, bottom, i);
            }

            drawAxes(e, top, bottom);
        }

        void drawIndividualSamples(PaintEventArgs e, float rectTop, float rectBottom, int channel)
        {
            int lower = Math.Max(0, audioData.CurrentSample - WindowLengthSamples / 2);
            int upper = Math.Min(audioData.GetChannel(0).Length, audioData.CurrentSample + WindowLengthSamples / 2);

            AudioSlice channelData = audioData.GetChannel(channel);

            for (int position = lower; position < upper; position++)
            {
                float x = getWaveformX(position);
                float y = getWaveformY(channelData[position], rectTop, rectBottom);

                e.Graphics.DrawLine(WAVEPEN, x, getWaveformY(0, rectTop, rectBottom), x, y);

                if (position > lower)
                {
                    e.Graphics.DrawLine(WAVEPEN, getWaveformX(position - 1), getWaveformY(channelData[position - 1], rectTop, rectBottom), x, y);
                }
            }

            drawStdevStatistics(e, rectTop, rectBottom, lower, upper, channelData.Slice);
        }

        private void drawStdevStatistics(PaintEventArgs e, float rectTop, float rectBottom, int lower, int upper, Slice<float> channelData)
        {
            Slice<float> range = channelData.GetSlice(lower, upper);
            float mean = FloatSliceUtil.Average(range, false);
            float meanAbs = FloatSliceUtil.Average(range, true);
            float stdev = FloatSliceUtil.StdDev(range);

            //e.Graphics.DrawString("Av = " + mean.ToString("0.0000"), textFont, Brushes.Black, new PointF(ClientRectangle.Left, ClientRectangle.Top + 100));
            //e.Graphics.DrawString("STDev = " + stdev.ToString("0.0000"), textFont, Brushes.Black, new PointF(ClientRectangle.Left, ClientRectangle.Top + 140));
            float meanY = getWaveformY(mean, rectTop, rectBottom);

            e.Graphics.DrawLine(Pens.Orange, ClientRectangle.Left, meanY, ClientRectangle.Right, meanY);

            drawStdevGradiation(e, rectTop, rectBottom, mean, stdev, Pens.Orange);
            drawStdevGradiation(e, rectTop, rectBottom, mean, meanAbs, Pens.Aqua);
        }

        private void drawStdevGradiation(PaintEventArgs e, float rectTop, float rectBottom, float mean, float stdev, Pen pen)
        {
            for (int i = 1; i <= 6; i++)
            {
                float stdevY = getWaveformY(mean + (float)i * stdev, rectTop, rectBottom);
                if (stdevY < rectTop + 10)
                    break;
                if (stdevY > rectBottom - 10)
                    break;

                e.Graphics.DrawLine(pen, ClientRectangle.Left, stdevY, ClientRectangle.Right, stdevY);
                e.Graphics.DrawString(i.ToString(), textFont, Brushes.Aqua, new PointF(ClientRectangle.X + ClientRectangle.Width / 2 - 40, stdevY));
            }
        }

        void drawCondensedWaveform(PaintEventArgs e, float rectTop, float rectBottom, int channel)
        {
            Slice<float> data = audioData.GetChannel(channel).Slice;

            for (float x = ClientRectangle.Left; x < ClientRectangle.Right; x += 1)
            {
                int a = waveformToDataX(x);
                if (a < 0)
                    continue;
                if (a >= audioData.Length)
                    break;

                int samplesPerPixel = audioData.ToSample(SecondsPerPixel);
                int b = a + samplesPerPixel;

                float low = data[a];
                float high = low;

                bool reachedEnd = false;
                for (int i = a + 1; i < b; i++)
                {
                    if (i >= data.Length)
                    {
                        reachedEnd = true;
                        break;
                    }

                    low = Math.Min(low, data[i]);
                    high = Math.Max(high, data[i]);
                }

                e.Graphics.DrawLine(
                    WAVEPEN,
                    x,
                    getWaveformY(low, rectTop, rectBottom),
                    x,
                    getWaveformY(high, rectTop, rectBottom));

                if (reachedEnd)
                {
                    break;
                }
            }
        }

        void drawAxes(PaintEventArgs e, float rectTop, float rectBottom)
        {
            e.Graphics.DrawString(viewportMax.ToString("0.00"), textFont, Brushes.LimeGreen, ClientRectangle.Left, rectTop);
            e.Graphics.DrawString(viewportMax.ToString("0.00"), textFont, Brushes.LimeGreen, ClientRectangle.Left, rectBottom - 20);
        }

        void drawTimingPoints(PaintEventArgs e)
        {
            if(timingPoints == null || timingPoints.Count == 0)
                return;

            double lowerBound = audioData.CurrentSampleSeconds - WindowLengthSeconds / 2.0;
            double upperBound = audioData.CurrentSampleSeconds + WindowLengthSeconds / 2.0;
            int startIndex = timingPoints.FirstVisible(lowerBound);

            double prevTime = timingPoints[Math.Max(startIndex - 1, 0)].TimeSeconds;

            for (int i = startIndex; i < timingPoints.Count; i++)
            {
                TimingPoint tp = timingPoints[i];

                if (tp.TimeSeconds > upperBound)
                    break;

                float x = getWaveformX(tp.TimeSeconds);

                drawingPen.Color = tp.Color;
                textBrush.Color = tp.Color;

                e.Graphics.DrawLine(drawingPen, x, ClientRectangle.Top, x, ClientRectangle.Bottom - 60);

                string desc = formatTimingPoint(prevTime, tp);

                e.Graphics.DrawString(desc, textFont, textBrush, new PointF(x, ClientRectangle.Bottom - 20), format);
                e.Graphics.DrawString("W:" + tp.Weight.ToString("0.000"), textFont, Brushes.Red, new PointF(x, ClientRectangle.Bottom - 40), format);

                prevTime = tp.TimeSeconds;
            }
        }

        private static string formatTimingPoint(double prevTime, TimingPoint tp)
        {
            return "[" + tp.BPM.ToString("0.00") + "," + tp.TimeSeconds.ToString("0.00") + "]" +
                                        "(+ " + (tp.TimeSeconds - prevTime).ToString("0.000") + "s)";
        }

        private void drawCurrentPositionText(PaintEventArgs e)
        {
            string currentTime = audioData.CurrentSampleSeconds.ToString("0.000") + "seconds" +
                            " / " + audioData.CurrentSample + "samples";

            e.Graphics.DrawString(currentTime, textFont, Brushes.Blue, new PointF(ClientRectangle.X + ClientRectangle.Width / 2, ClientRectangle.Top));
        }


        private void drawMidLine(PaintEventArgs e)
        {
            int mid = ClientRectangle.X + ClientRectangle.Width / 2;
            e.Graphics.DrawLine(Pens.Black, mid - 1, ClientRectangle.Top, mid - 1, ClientRectangle.Bottom);
            e.Graphics.DrawLine(Pens.Yellow, mid, ClientRectangle.Top, mid, ClientRectangle.Bottom);
            e.Graphics.DrawLine(Pens.Black, mid + 1, ClientRectangle.Top, mid + 1, ClientRectangle.Bottom);
        }


        float getWaveformY(float sample, float rectTop, float rectBottom)
        {
            float percent = -0.5f * MathUtilF.Clamp(sample / Math.Abs(viewportMax), -1, 1) + 0.5f;

            float y = rectTop + (rectBottom - rectTop) * percent;

            return y;
        }


        float getWaveformX(double time)
        {
            return getWaveformX(audioData.ToSample(time));
        }

        int waveformToDataX(float waveformX)
        {
            return audioData.CurrentSample - WindowLengthSamples / 2 + (int)(((waveformX / (float)ClientRectangle.Width) * (float)WindowLengthSamples));
        }

        float getWaveformX(int samples)
        {
            return ClientRectangle.X + ((samples - audioData.CurrentSample + WindowLengthSamples / 2) / (float)WindowLengthSamples) * ClientRectangle.Width;
        }


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
