using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SongBPMFinder
{
    public class DrawableTimingPoints : IDrawable
    {
        TimingPointList timingPoints;
        
        Font textFont;
        StringFormat format;
        Pen drawingPen;
        SolidBrush textBrush;

        public DrawableTimingPoints(TimingPointList timingPoints)
        {
            drawingPen = new Pen(Color.Gray, 3);
            textBrush = new SolidBrush(Color.Gray);

            format = new StringFormat();
            format.Alignment = StringAlignment.Center;

            textFont = new Font(SystemFonts.DefaultFont.FontFamily, 12.0f, FontStyle.Regular);

            this.timingPoints = timingPoints;
        }


        public void Draw(Control control, WaveformCoordinates coordinates, Graphics g)
        {
            if (timingPoints == null || timingPoints.Count == 0)
                return;

            Rectangle clientRectangle = control.ClientRectangle;

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

                g.DrawLine(drawingPen, x, clientRectangle.Top, x, clientRectangle.Bottom - 60);

                string desc = formatTimingPoint(prevTime, tp);

                g.DrawString(desc, textFont, textBrush, new PointF(x, clientRectangle.Bottom - 20), format);
                g.DrawString("W:" + tp.Weight.ToString("0.000"), textFont, Brushes.Red, new PointF(x, clientRectangle.Bottom - 40), format);

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
