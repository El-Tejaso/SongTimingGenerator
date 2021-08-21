using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SongBPMFinder
{
    public static class TimingUtil
    {
        public static TimingPointList GetTiming(AudioData data, AbstractBeatDetector beatDetector, ITimingGenerator timingGenerator)
        {
            SortedList<Beat>[] beats = beatDetector.GetEveryBeat(data);

            TimingPointList timing = timingGenerator.GenerateTiming(beats);

            return timing;
        }

        public static string GetTimingOsuString(AudioData data, AbstractBeatDetector beatDetector, ITimingGenerator timingGenerator)
        {
            return new OsuTimingPointFormatter().FormatTiming(GetTiming(data, beatDetector, timingGenerator));
        }
    }
}
