using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SongBPMFinder
{
    public class TimeSeries
    {
        public List<double> Times;
        public List<float> Values;

        public TimeSeries(List<double> times, List<float> values)
        {
            Times = times;
            Values = values;
        }

        public TimeSeries()
        {
            Times = new List<double>();
            Values = new List<float>();
        }
    }

    public class TimeSeriesDrawer
    {
        TimeSeries timeSeries;

        public TimeSeries TimeSeries {
            get { return timeSeries; }
            set {
                timeSeries = value;

                //something something
            }
        }

        Control winformControl;
        Pen linePen;
        WaveformCoordinates coordinates;


        Rectangle ClientRectangle { get => winformControl.ClientRectangle; }

        public TimeSeriesDrawer(Control winformControl, WaveformCoordinates coordinates)
        {
            this.winformControl = winformControl;
            this.coordinates = coordinates;

            linePen = new Pen(Color.Red);
        }


        public void DrawTimeSeries(Graphics g)
        {
            if (timeSeries == null)
                return;

            int firstVisible = 0;
            double windowLeftSeconds = coordinates.WindowLeftSeconds;
            while (firstVisible < timeSeries.Times.Count)
            {
                if (timeSeries.Times[firstVisible] >= windowLeftSeconds)
                    break;

                firstVisible++;
            }

            int top = ClientRectangle.Top;
            int bottom = ClientRectangle.Bottom;

            for(int i = firstVisible; i <  timeSeries.Times.Count-1; i++)
            {
                float x0 = coordinates.GetWaveformXSeconds(timeSeries.Times[i]);
                float x1 = coordinates.GetWaveformXSeconds(timeSeries.Times[i+1]);

                float y0 = coordinates.GetWaveformY(timeSeries.Values[i], top, bottom);
                float y1 = coordinates.GetWaveformY(timeSeries.Values[i + 1], top, bottom);

                g.DrawLine(linePen, x0, y0, x1, y1);
            }
        }
    }
}
