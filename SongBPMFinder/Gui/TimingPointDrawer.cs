using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SongBPMFinder
{
    public class TimingPointDrawer
    {
        TimingPointList timingPoints;
        public TimingPointList TimingPoints {
            get {
                return timingPoints;
            }

            set {
                timingPoints = value;
                //invoke event or someth
            }
        }

        Control client;
        Rectangle ClientRectangle { get => client.ClientRectangle; }


        WaveformCoordinates coordinates;

        Font textFont;
        StringFormat format;
        Pen drawingPen;
        SolidBrush textBrush;

        public TimingPointDrawer(Control client, Font font, WaveformCoordinates coordinates, TimingPointList tpList)
        {
            this.client = client;
            this.textFont = font;
            this.coordinates = coordinates;
            this.timingPoints = tpList;

            drawingPen = new Pen(Color.Gray, 3);
            textBrush = new SolidBrush(Color.Gray);

            format = new StringFormat();
            format.Alignment = StringAlignment.Center;
        }


        public void DrawTimingPoints(Graphics g)
        {
            if (timingPoints == null || timingPoints.Count == 0)
                return;


            int startIndex = timingPoints.FirstVisible(coordinates.WindowLeftSeconds);
            double prevTime = timingPoints[Math.Max(startIndex - 1, 0)].TimeSeconds;

            for (int i = startIndex; i < timingPoints.Count; i++)
            {
                TimingPoint tp = timingPoints[i];

                if (tp.TimeSeconds > coordinates.WindowRightSeconds)
                    break;

                float x = coordinates.GetWaveformXSeconds(tp.TimeSeconds);

                drawingPen.Color = tp.Color;
                textBrush.Color = tp.Color;

                g.DrawLine(drawingPen, x, ClientRectangle.Top, x, ClientRectangle.Bottom - 60);

                string desc = formatTimingPoint(prevTime, tp);

                g.DrawString(desc, textFont, textBrush, new PointF(x, ClientRectangle.Bottom - 20), format);
                g.DrawString("W:" + tp.Weight.ToString("0.000"), textFont, Brushes.Red, new PointF(x, ClientRectangle.Bottom - 40), format);

                prevTime = tp.TimeSeconds;
            }
        }

        private static string formatTimingPoint(double prevTime, TimingPoint tp)
        {
            return "[" + tp.BPM.ToString("0.00") + "," + tp.TimeSeconds.ToString("0.00") + "]" +
                                        "(+ " + (tp.TimeSeconds - prevTime).ToString("0.000") + "s)";
        }
    }
}
