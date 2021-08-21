using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SongBPMFinder
{
    public class TimingPipeline
    {
        List<TimeSeries> debugTimeSeries = new List<TimeSeries>();

        public List<TimeSeries> DebugTimeSeries {
            get {
                return debugTimeSeries;
            }
        }

        public TimingPointList TimeSong(AudioData audio)
        {
            SortedList<Beat>[] beats = new DefaultBeatDetector(debugTimeSeries).GetEveryBeat(audio);

            TimingPointList timingPoints = new TimingGenerator(debugTimeSeries).GenerateTiming(beats);

            return timingPoints;
        }
    }
}
