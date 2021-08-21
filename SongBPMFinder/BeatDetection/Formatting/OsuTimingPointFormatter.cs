using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SongBPMFinder
{
    public class OsuTimingPointFormatter : ITimingPointFormatter
    {
        public string FormatTiming(TimingPointList timingPoints)
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < timingPoints.Count; i++)
            {
                string timingPointString = TimingPointToOsuTimingPointString(timingPoints[i]);

                sb.Append(timingPointString + "\n");
            }

            return sb.ToString();
        }


        /// <summary>
        /// Osu timing points are of the following format: 
        /// 
        /// "[{Offset in ms}, {beat length in ms}(60000 / BPM), {time-signature} / 4, 2(?), 
        /// {sampleset}, {volume %}, 1(?), 0(?)]"
        /// 
        /// We only care about the offset in milliseconds, and the bpm. so to us, this will look like:
        /// 
        /// "[{Offset in ms}, {beat length in ms}(60000 / BPM)," + some_other_garbage = "int,int,bool(1/0), bool(1/0)"
        /// </summary>
        private static string TimingPointToOsuTimingPointString(TimingPoint tp)
        {
            return "" + tp.OffsetMilliseconds + "," + (60000 / tp.BPM) + ",4,2,1,100,1,0";
        }
    }
}