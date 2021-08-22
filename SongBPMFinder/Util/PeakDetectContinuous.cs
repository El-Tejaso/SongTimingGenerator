using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SongBPMFinder
{
    /// <summary>
    /// This class doesn't work.  I will delete it later if I really can't fix it
    /// </summary>
    public static class PeakDetectContinuous
    {
        /// <summary>
        /// Original algorithm btw.
        /// 
        /// Works best on smooth data with a large number of datapoints.
        /// Use moving average/some other smoothing functions on the input if it isn't smooth.
        /// 
        /// This function is best used with a foreach statement.
        /// 
        /// this function should give you the flexibility to implement domain-specific peak scaling yourself.
        /// </summary>
        public static IEnumerable<TimeSeriesPeak> DetectPeaks(ReadOnlyMemory<float> values, double deltaTime, double upwardsGradientThreshold, 
            double downwardsGradientThreshold, double minInflectionTime)
        {
            //Needs to start at one because the process of finding gradients
            //looks at the previous sample.
            int ascentStart = 1;

            while (insideBounds(values, ascentStart))
            {
                ascentStart = findWhenAscentStarts(ref values, deltaTime, upwardsGradientThreshold, ascentStart);

                int ascentTaperPoint = findWhenAscentTapers(ref values, deltaTime, upwardsGradientThreshold, ascentStart);

                int peakOrMoreAscent;
                bool isAscent;
                findNextPeakOrAscent(ref values, deltaTime, upwardsGradientThreshold, ascentTaperPoint, 
                    out peakOrMoreAscent, out isAscent);

                if (isAscent)
                {
                    ascentStart = peakOrMoreAscent;
                    continue;
                }

                int peakIndex = peakOrMoreAscent;

                int ascentOrDescentPoint;
                findNextAscentOrDescent(ref values, deltaTime, upwardsGradientThreshold, 
                    downwardsGradientThreshold, peakIndex, out ascentOrDescentPoint, out isAscent);

                if (isAscent)
                {
                    ascentStart = peakOrMoreAscent;
                    continue;
                }

                int descentStartPoint = ascentOrDescentPoint;

                int descentTaperPoint = findWhenDescentTapers(ref values, deltaTime, downwardsGradientThreshold, descentStartPoint);


                double inflectionTime = (descentStartPoint - ascentTaperPoint) * deltaTime;
                if (inflectionTime > minInflectionTime)
                {
                    //We can wearly exit here and continue since we took too long to start dipping.
                    //In other words, our curve looks more like /TTTTTTT\ rather than /\ 
                }

                float ascentStartV = values.Span[ascentStart];
                float peakV = values.Span[peakIndex];
                float descentEndV = values.Span[descentTaperPoint];

                const float startWeight = 1, endWeight = 1;
                float peakHeight = (startWeight * (peakV - ascentStartV) + endWeight * (peakV - descentEndV)) / (startWeight + endWeight);

                yield return new TimeSeriesPeak(peakIndex * deltaTime, peakHeight, peakIndex);

                ascentStart = descentTaperPoint + 1;
            }
        }

        private static int findWhenAscentTapers(ref ReadOnlyMemory<float> values, double deltaTime, double upwardsGradientThreshold, int i)
        {
            while (insideBounds(values, i) && isAscending(upwardsGradientThreshold, ref values, deltaTime, i))
            {
                i++;
            }

            return i;
        }


        private static int findWhenDescentTapers(ref ReadOnlyMemory<float> values, double deltaTime, double downwardsGradientThreshold, int i)
        {
            while (insideBounds(values, i) && isDescending(downwardsGradientThreshold, ref values, deltaTime, i))
            {
                i++;
            }

            return i;
        }


        private static int findWhenAscentStarts(ref ReadOnlyMemory<float> values, double deltaTime, double upwardsGradientThreshold, int i)
        {
            while (insideBounds(values, i) && !isAscending(upwardsGradientThreshold, ref values, deltaTime, i))
            {
                i++;
            }

            return i;
        }


        private static void findNextPeakOrAscent(ref ReadOnlyMemory<float> values, double deltaTime, double upwardsGradientThreshold, int i,
            out int index, out bool isAscent)
        {
            bool notAscending = true;
            while (
                insideBounds(values, i) &&
                (isPeaking(ref values, deltaTime, i)) &&
                (notAscending = !isAscending(upwardsGradientThreshold, ref values, deltaTime, i))
            )
            {
                i++;
            }

            index = i;
            isAscent = !notAscending;
        }

        private static void findNextAscentOrDescent(ref ReadOnlyMemory<float> values, double deltaTime, double upwardsGradientThreshold,
            double downwardsGradientThreshold, int i,
            out int index, out bool isAscent)
        {
            bool notAscending = false;
            while (
                insideBounds(values, i) &&
                (!isDescending(downwardsGradientThreshold, ref values, deltaTime, i)) &&
                (notAscending = !isAscending(upwardsGradientThreshold, ref values, deltaTime, i))
            )
            {
                i++;
            }

            index = i;
            isAscent = !notAscending;
        }


        private static bool insideBounds(ReadOnlyMemory<float> values, int i)
        {
            return i < values.Length - 1;
        }

        private static bool isAscending(double upwardsGradientThreshold, ref ReadOnlyMemory<float> values, double deltaTime, int i)
        {
            return (gradient(ref values, i, deltaTime) > upwardsGradientThreshold);
        }

        private static bool isPeaking(ref ReadOnlyMemory<float> values, double deltaTime, int i)
        {
            return gradient(ref values, i, deltaTime) < 0;
        }

        private static bool isDescending(double downwardsGradientThreshold, ref ReadOnlyMemory<float> values, double deltaTime, int i)
        {
            return gradient(ref values, i, deltaTime) < -downwardsGradientThreshold;
        }

        private static float gradient(ref ReadOnlyMemory<float> array, int i, double deltaTime)
        {
            return (array.Span[i] - array.Span[i - 1]) / (float)deltaTime;
        }
    }
}
