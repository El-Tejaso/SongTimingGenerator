using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SongBPMFinder
{
    public class TimingPointList
    {
        SortedList<TimingPoint> timingPoints = new SortedList<TimingPoint>();
        List<TimeSeries> debugTimeSeries;

        public TimingPointList(List<TimeSeries> debugTimeSeries)
        {
            this.debugTimeSeries = debugTimeSeries;
        }

        public int Count => timingPoints.Count;
        public TimingPoint this[int index] => timingPoints[index];

        public TimingPointList(SortedList<TimingPoint> timingPoints)
        {
            this.timingPoints = timingPoints;
        }

        public int FirstVisible(double time)
        {
            for(int i = 0; i < timingPoints.Count; i++)
            {
                if (timingPoints[i].TimeSeconds > time)
                    return i;
            }

            return timingPoints.Count;
        }
    }
}
