using System;

namespace SongBPMFinder
{
    public struct Beat : IComparable<Beat>
    {
        public int ArrayIndex { get; set; }
        public double Time;
        public float Weight;

        public Beat(double time, float weight)
        {
            Time = time;
            Weight = weight;
            ArrayIndex = 0;
        }

        public int CompareTo(Beat other)
        {
            return Time.CompareTo(other.Time);
        }
    }
}
