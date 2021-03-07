using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using SongBPMFinder.Util;

namespace SongBPMFinder.Audio.Timing
{
    public class TimingPoint : IComparable<TimingPoint>
    {

        public TimingPoint(double bpm, double offset, Color col)
        {
            BPM = bpm;
            OffsetSeconds = offset;
            Color = col;
        }

        public TimingPoint(double bpm, double offset)
        {
            BPM = bpm;
            OffsetSeconds = offset;
        }

        public double BPM;
        public double OffsetSeconds;
        //Purely a debugging feature
        public Color Color = Color.Red;

        public long OffsetMilliseconds {
            get => (long)(OffsetSeconds * 1000);
        }

        public int CompareTo(TimingPoint obj)
        {
            return OffsetSeconds.CompareTo(obj.OffsetSeconds);
        }


        /// <summary>
        /// Returns if one BPM is an integer multiple of another
        /// </summary>
        /// <param name="bpm"></param>
        /// <param name="tolerance"></param>
        /// <returns></returns>
        public bool IsEquivelantBPM(double bpm, double tolerance = 0.000001)
        {
            return QuickMafs.IsIntegerMultiple(BPM, bpm, tolerance);
        }

        /// <summary>
        /// Returns the snap value of another point in time.
        /// 1 corresponds to 1/1 beatsnap
        /// 2 corresponds to 1/2 beatsnap
        /// 3 corresponds to 1/3 beatsnap
        /// and so on
        /// Osu! mappers (and possibly players) will be familiar with this concept
        /// -1 corresponds to no snap
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public int SnapValue(double t, double tolerance = 0.000001)
        {
            if(t < (OffsetSeconds - tolerance))
            {
                return -1;
            }


            double bpmDelta = 60.0 / BPM;
            double actualDelta = t - OffsetSeconds;

            double snapRemainder = (actualDelta % bpmDelta) % 1.0;

            if (Math.Abs(snapRemainder) < tolerance) return 1;

            double snapMultiple = 1.0/snapRemainder;
            double snapMultipleRemainder = snapMultiple % 1.0;

            if ((snapMultipleRemainder < tolerance)||((1.0 - snapMultipleRemainder) < tolerance))
            {
                int multiple = (int)Math.Round(snapMultiple);

                //1/16 snap is the greatest viable beatsnap in osu!
                //but with all due respect, 1/8 snap is the only respectable snap
                if (multiple > 8) return -1;

                return multiple;
            }

            return -1;
        }
    }
}
