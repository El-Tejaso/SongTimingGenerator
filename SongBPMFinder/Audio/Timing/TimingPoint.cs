using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SongBPMFinder.Audio.Timing
{
    public struct TimingPoint : IComparable<TimingPoint>
    {
        public TimingPoint(double bpm, double offset)
        {
            BPM = bpm;
            OffsetSeconds = offset;
        }

        public double BPM;
        public double OffsetSeconds;
        public long OffsetMilliseconds {
            get => (long)(OffsetSeconds * 1000);
        }

        public int CompareTo(TimingPoint obj)
        {
            return OffsetSeconds.CompareTo(obj.OffsetSeconds);
        }
    }
}
