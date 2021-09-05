using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SongBPMFinder
{

    public class DrawableTimeSeries : IDrawable
    {
        Pen linePen;
        TimeSeries timeSeries;
        int drawWindowStart = 0;

        public DrawableTimeSeries(TimeSeries timeSeries)
        {
            linePen = new Pen(Color.Red);
            this.timeSeries = timeSeries;
        }


        public void Draw(Control control,  WaveformCoordinates coordinates, Graphics g)
        {
            Rectangle clientRectangle = control.ClientRectangle;

            linePen.Color = timeSeries.Color;
            linePen.Width = timeSeries.Width;


            double windowLeftSeconds = coordinates.WindowLeftSeconds;
            alignDrawWindowStartToTime(windowLeftSeconds);

            int top = clientRectangle.Top;
            int bottom = clientRectangle.Bottom - 80;

            for (int i = drawWindowStart; i + 1 < timeSeries.Times.Length; i++)
            {
                float x0 = coordinates.GetWaveformXSeconds(timeSeries.Times[i]);
                float x1 = coordinates.GetWaveformXSeconds(timeSeries.Times[i + 1]);

                float y0 = coordinates.GetWaveformY(timeSeries.Values[i], top, bottom);
                float y1 = coordinates.GetWaveformY(timeSeries.Values[i + 1], top, bottom);

                try
                {
                    g.DrawLine(linePen, x0, y0, x1, y1);

                    const int rectSize = 1;

                    if (i == 0)
                    {
                        //g.DrawRectangle(linePen, x0 - rectSize, y0 - rectSize, 2*rectSize, 2 * rectSize);
                    }

                    //g.DrawRectangle(linePen, x1 - rectSize, y1 - rectSize, 2 * rectSize, 2 * rectSize);
                }
                catch (Exception e)
                {
                    break;
                }
            }
        }

        private void alignDrawWindowStartToTime(double windowLeftSeconds)
        {
            while (drawWindowStart-1 >= 0 && timeSeries.Times[drawWindowStart-1] > windowLeftSeconds)
            {
                drawWindowStart--;
            }

            while (drawWindowStart+1 < timeSeries.Times.Length && timeSeries.Times[drawWindowStart+1] < windowLeftSeconds)
            {
                drawWindowStart++;
            }
        }
    }
}
