using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SongBPMFinder
{

    public class TimeSeriesDrawer
    {
        List<TimeSeries> timeSeriesList = new List<TimeSeries>();

        Control winformControl;
        WaveformCoordinates coordinates;
        Pen linePen;

        Rectangle ClientRectangle { get => winformControl.ClientRectangle; }
        public int SeriesCount { get => timeSeriesList.Count; }

        public TimeSeriesDrawer(Control winformControl, WaveformCoordinates coordinates)
        {
            this.winformControl = winformControl;
            this.coordinates = coordinates;

            linePen = new Pen(Color.Red);
        }

        public void AddTimeSeries(TimeSeries t)
        {
            timeSeriesList.Add(t);
        }


        public void RemoveTimeSeries(TimeSeries t)
        {
            timeSeriesList.Remove(t);
        }

        public void ClearTimeSeriesList()
        {
            timeSeriesList.Clear();
        }

        public void DrawTimeSeries(Graphics g)
        {
            for(int i = 0; i < timeSeriesList.Count; i++)
            {
                drawSingleTimeSeries(g, i);
            }
        }

        int lastWindowStart = 0;

        public void drawSingleTimeSeries(Graphics g, int timeSeriesIndex)
        {
            TimeSeries timeSeries = timeSeriesList[timeSeriesIndex];
            linePen.Color = timeSeries.Color;
            linePen.Width = timeSeries.Width;

            int firstVisible = lastWindowStart;
            double windowLeftSeconds = coordinates.WindowLeftSeconds;


            while (firstVisible > 0 && timeSeries.Times[firstVisible] > windowLeftSeconds)
            {
                firstVisible--;
            }

            while (firstVisible < timeSeries.Times.Length && timeSeries.Times[firstVisible] < windowLeftSeconds)
            {
                firstVisible++;
            }

            if (firstVisible > timeSeries.Times.Length)
            {
                firstVisible = firstVisible;
            }

            lastWindowStart = firstVisible;

            int top = ClientRectangle.Top;
            int bottom = ClientRectangle.Bottom;

            for(int i = firstVisible; i+1 <  timeSeries.Times.Length; i++)
            {
                float x0 = coordinates.GetWaveformXSeconds(timeSeries.Times[i]);
                float x1 = coordinates.GetWaveformXSeconds(timeSeries.Times[i+1]);

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
                catch(Exception e)
                {
                    break;
                }
            }
        }

    }
}
