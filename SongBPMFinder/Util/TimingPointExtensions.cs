using System;

namespace SongBPMFinder
{
    public static class TimingPointExtensions
    {
        private static readonly int[] ValidBeatsnapDivisors = new int[]
        {
            1,2,3,4,5,6,7,8,12,16
        };

        public static bool IsEquivelantBPM(this TimingPoint self, double bpm, double tolerance = 0.000001)
        {
            return MathUtilF.IsIntegerMultiple(self.BPM, bpm, tolerance);
        }

        /// <summary>
        /// if the time from this timing point to time t can be divided by some fraction of
        /// this timing point's beat length such that the remainder is less than some tolerance value, 
        /// then that divisor is returned.
        /// 
        /// osu! mappers and possibly other rhythm game charters would know of this concept as 'beat-snap'.
        /// 
        /// only whole integers from 1 to 16 are considered valid beat-snaps.
        /// 
        /// If the time provided is not a valid snap position, -1 is returned
        /// </summary>
        private static int SnapValue(this TimingPoint self, double t, double tolerance = 0.000001)
        {
            double relativeTime = t - self.TimeSeconds;

            if (relativeTime < 0)
                return -1;

            double beatLengthSeconds = 60.0 / self.BPM;

            for (int i = 0; i < ValidBeatsnapDivisors.Length; i++)
            {
                double fractionOfBeatlength = beatLengthSeconds / (double)ValidBeatsnapDivisors[i];
                double snapRemainder = (relativeTime % fractionOfBeatlength);
                if (Math.Abs(snapRemainder) < tolerance)
                    return ValidBeatsnapDivisors[i];
            }

            return -1;
        }
    }
}
