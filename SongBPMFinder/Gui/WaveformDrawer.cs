using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SongBPMFinder
{
    /// <summary>
    /// Only one waveform at a time. Dont make this an IDrawable or something
    /// </summary>
    public class WaveformDrawer
    {
        AudioData audioData;
        public AudioData AudioData {
            get {
                return audioData;
            }
            set {
                audioData = value;
                //invoke event or someth
            }
        }

        Control winformControl;
        Font textFont;
        Pen wavePen;

        WaveformCoordinates coordinates;

        public bool ForceIndividualView { get; set; }

        Rectangle ClientRectangle { get => winformControl.ClientRectangle; }


        public WaveformDrawer(Control winformControl, Font font, AudioData audioData, WaveformCoordinates coordinates)
        {
            this.winformControl = winformControl;
            this.textFont = font;
            this.audioData = audioData;
            this.coordinates = coordinates;

            wavePen = new Pen(Color.FromArgb(155, 0, 0, 255));
        }

        public void DrawAudioWaveform(Graphics g)
        {
            for (int i = 0; i < audioData.NumChannels; i++)
            {
                drawAudioChannelWaveform(g, i);
            }
        }

        private void drawAudioChannelWaveform(Graphics g, int channel)
        {
            int height = (int)((ClientRectangle.Height-80) / (float)audioData.NumChannels);
            int top = ClientRectangle.Top + (int)(channel * height);
            int bottom = top + height;

            Rectangle region = new Rectangle(ClientRectangle.X, top, ClientRectangle.Width, bottom - top);
            AudioChannel data = audioData[0];

            int samplesPerPixel = audioData.SecondsToSamples(coordinates.SecondsPerPixel);
            if (ForceIndividualView || samplesPerPixel <= 1)
            {
                drawIndividualSamples(region, data, g);
            }
            else
            {
                drawCondensedWaveform(region, data, g);
            }

            drawAxes(region, g);
        }

        void drawIndividualSamples(Rectangle region, AudioChannel entireChannel, Graphics g)
        {
            int leftMost = coordinates.VeryLeftSample;
            int rightMost = coordinates.VeryRightSample;
            if (rightMost - leftMost == 0)
                return;

            float rectTop = region.Top;
            float rectBottom = region.Bottom;

            for (int position = leftMost; position < rightMost; position++)
            {
                float x = coordinates.GetWaveformXSamples(position);
                float y = coordinates.GetWaveformY(entireChannel[position], rectTop, rectBottom);

                g.DrawLine(wavePen, x, coordinates.GetWaveformY(0, rectTop, rectBottom), x, y);

                if (position > leftMost)
                {
                    g.DrawLine(
                        wavePen, coordinates.GetWaveformXSamples(position - 1), 
                        coordinates.GetWaveformY(entireChannel[position - 1], rectTop, rectBottom), 
                        x, y);
                }
            }

            drawStdevStatistics(g, region, entireChannel.GetSlice(coordinates.VeryLeftSample, coordinates.VeryRightSample));
        }

        private void drawStdevStatistics(Graphics g, Rectangle region, Span<float> specificRange)
        {
            float mean = MathUtilSpanF.Average(specificRange);
            float meanAbs = MathUtilSpanF.Mean(specificRange, Math.Abs);
            float stdev = MathUtilSpanF.StandardDeviation(specificRange);

            float rectTop = region.Top;
            float rectBottom = region.Bottom;

            //e.Graphics.DrawString("Av = " + mean.ToString("0.0000"), textFont, Brushes.Black, new PointF(ClientRectangle.Left, ClientRectangle.Top + 100));
            //e.Graphics.DrawString("STDev = " + stdev.ToString("0.0000"), textFont, Brushes.Black, new PointF(ClientRectangle.Left, ClientRectangle.Top + 140));
            float meanY = coordinates.GetWaveformY(mean, rectTop, rectBottom);

            if (float.IsNaN(meanY))
                return;

            g.DrawLine(Pens.Orange, ClientRectangle.Left, meanY, ClientRectangle.Right, meanY);

            drawStdevGradiation(g, rectTop, rectBottom, mean, stdev, Pens.Orange);
            drawStdevGradiation(g, rectTop, rectBottom, mean, meanAbs, Pens.Aqua);
        }

        private void drawStdevGradiation(Graphics g, float rectTop, float rectBottom, float mean, float stdev, Pen pen)
        {
            for (int i = 1; i <= 6; i++)
            {
                float stdevY = coordinates.GetWaveformY(mean + (float)i * stdev, rectTop, rectBottom);
                if (stdevY < rectTop + 10)
                    break;
                if (stdevY > rectBottom - 10)
                    break;

                g.DrawLine(pen, ClientRectangle.Left, stdevY, ClientRectangle.Right, stdevY);
                g.DrawString(i.ToString(), textFont, Brushes.Aqua, new PointF(ClientRectangle.X + ClientRectangle.Width / 2 - 40, stdevY));
            }
        }

        void drawCondensedWaveform(Rectangle region, AudioChannel entireChannel, Graphics g)
        {
            float rectTop = region.Top;
            float rectBottom = region.Bottom;

            for (float x = region.Left; x < region.Right; x++)
            {
                int rangeStart = coordinates.WaveformToDataX(x);

                if (rangeStart < 0)
                    continue;
                if (rangeStart >= entireChannel.Length)
                {
                    int z = 3;
                    break;
                }

                int samplesPerPixel = audioData.SecondsToSamples(coordinates.SecondsPerPixel);
                int rangeEnd = rangeStart + samplesPerPixel;

                float low = 0;
                float high = low;

                bool reachedEnd = false;

                //Some sort of optimization
                int stride = Math.Max(1, (rangeEnd - rangeStart) / 50);

                for (int i = rangeStart; i < rangeEnd; i+= stride)
                {
                    if (i >= entireChannel.Length)
                    {
                        reachedEnd = true;
                        break;
                    }

                    low = Math.Min(low, entireChannel[i]);
                    high = Math.Max(high, entireChannel[i]);
                }

                g.DrawLine(
                    wavePen,
                    x,
                    coordinates.GetWaveformY(low, rectTop, rectBottom),
                    x,
                    coordinates.GetWaveformY(high, rectTop, rectBottom));

                if(reachedEnd)
                {
                    break;
                }
            }
        }

        void drawAxes(Rectangle region, Graphics g)
        {
            float vmax = coordinates.ViewportMax;
            g.DrawString(vmax.ToString("+0.00"), textFont, Brushes.Black, region.Left, region.Top);
            g.DrawString(vmax.ToString("-0.00"), textFont, Brushes.Black, region.Left, region.Bottom - 20);
        }
    }
}
