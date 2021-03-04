using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SongBPMFinder.Util;

namespace SongBPMFinder.Audio.Timing
{
    public class TimingPointList
    {
        public enum TimingFormat
        {
            Osu
        };

        List<TimingPoint> timingPoints;

        public List<TimingPoint> List => timingPoints;

        public int First(double time)
        {
            for(int i = 0; i < timingPoints.Count; i++)
            {
                if (timingPoints[i].OffsetSeconds >= time)
                    return i;
            }
            return timingPoints.Count;
        }

        public static List<TimingPoint> RemoveDoubles(List<TimingPoint> timingPoints, double tolerance = 0.0001)
        {
            List<TimingPoint> cleanList = new List<TimingPoint>();
            cleanList.Capacity = timingPoints.Count;

            int doubles = 0;
            for (int i = 0; i < timingPoints.Count - 1; i++)
            {
                double a = timingPoints[i].OffsetSeconds;
                double b = timingPoints[i + 1].OffsetSeconds;

                if (Math.Abs(a - b) >= tolerance)
                {
                    cleanList.Add(timingPoints[i]);
                }
                else
                {
                    doubles++;
                }
            }

            return cleanList;
        }

        public static List<TimingPoint> CalculateBpms(List<TimingPoint> timingPoints)
        {


            return timingPoints;
        }

        public static List<TimingPoint> Simplify(List<TimingPoint> timingPoints)
        {
            

            return timingPoints;
        }

        public void RemoveDoubles(double tolerance = 0.0001){
            int oldCount = timingPoints.Count;
            timingPoints = TimingPointList.RemoveDoubles(timingPoints);

            int doubles = oldCount - timingPoints.Count;
            //if(doubles>0)
            Logger.Log("" + doubles + " duplicate timing points removed");
		}

        public TimingPointList(List<TimingPoint> timingPointsIn)
        {
            timingPoints = timingPointsIn;
            timingPoints.Sort();
			RemoveDoubles();
        }

        public string GetOsuString() {
            StringBuilder sb = new StringBuilder();

            for(int i = 0; i < timingPoints.Count; i++)
            {
                TimingPoint tp = timingPoints[i];
                //sb.Append("[Offset in MS, DeltaTime in MS(60000 / BPM), TimeSig / 4, 2, Sampleset, volume %, 1, 0]");
                //735,324.324324324324,4,2,22,42,1,0
                sb.Append("" + tp.OffsetMilliseconds + "," + (60000 / tp.BPM) + ",4,2,1,100,1,0\n");
            }

            return sb.ToString();
        }

        public string ToString(TimingFormat format)
        {
            switch (format)
            {
                default:
                {
                    return GetOsuString();
                }
            }
        }
    }
}
