using System;
using System.Drawing;

namespace SongBPMFinder
{
    public struct TimingPoint : IComparable<TimingPoint>
    {
        public readonly double BPM;
        public readonly double TimeSeconds;

        //Could be usefull in some algorithms
        public readonly double Weight;

        //Purely a debugging feature
        public readonly Color Color;

        public TimingPoint(double bPM, double offsetSeconds)
            : this(bPM, offsetSeconds, 1, Conventions.TimingPointColor)
        { }

        public TimingPoint(double bPM, double offsetSeconds, double weight, Color color)
        {
            BPM = bPM;
            TimeSeconds = offsetSeconds;
            Weight = weight;
            Color = color;
        }

        public long OffsetMilliseconds {
            get => (long)(TimeSeconds * 1000);
        }

        public int CompareTo(TimingPoint obj)
        {
            return TimeSeconds.CompareTo(obj.TimeSeconds);
        }
    }
}
