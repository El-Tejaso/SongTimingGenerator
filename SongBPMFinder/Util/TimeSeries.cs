using System;
using System.Collections.Generic;
using System.Drawing;

namespace SongBPMFinder
{
    public class TimeSeriesBuilder
    {
        public SortedList<double> Times = new SortedList<double>();
        public List<float> Values = new List<float>();

        public void Add(double t, float v)
        {
            int index = Times.Add(t);
            Values.Insert(index, v);
        }

        public TimeSeries ToTimeSeries()
        {
            return new TimeSeries(Times.ToArray(), Values.ToArray());
        }
    }

    public class TimeSeries
    {
        private double[] times;
        private float[] values;

        public Color Color;
        public int Width = 2;

        public double[] Times {
            get {
                return times;
            }

            private set {
                times = value;
            }
        }
        public float[] Values {
            get {
                return values;
            }

            private set {
                values = value;
            }
        }

        public TimeSeries(double[] times, float[] values)
        {
            Times = times;
            Values = values;
        }

        public void Normalize()
        {
            float max = MathUtilSpanF.Max(Values);
            MathUtilSpanF.Divide(Values, max, Values);
        }

        public void AdaptiveNormalize(double windowSizeSeconds)
        {
            if (Times.Length == 0)
                return;

            double halfWindowSize = windowSizeSeconds / 2;

            int rangeStart = 0;
            int rangeEnd = 0;

            float[] newValues = new float[Values.Length];

            for (int currValueIndex = 0; currValueIndex < Values.Length; currValueIndex++)
            {
                double currentTime = Times[currValueIndex];

                double lowerBound = currentTime - halfWindowSize;
                while (rangeStart+1 < Times.Length && Times[rangeStart] < lowerBound)
                    rangeStart++;

                double upperBound = currentTime + halfWindowSize;
                //the Times[rangeEnd+1] is different to the previous Times[rangeStart] on purpose
                while (rangeEnd+1 < Times.Length && Times[rangeEnd+1] < upperBound)
                    rangeEnd++;


                //Not necessarily the same as windowSizeSeconds
                double currentWindowSize = Times[rangeStart] + (Times[rangeEnd] - Times[rangeStart]) / 2.0;

                float maxInRange = 0;

                for(int j = rangeStart; j <= rangeEnd; j++)
                {
                    float normalizedTime = (float)(Times[j] - Times[currValueIndex] / currentWindowSize);
                    float gaussianFactor = MathUtilF.Gaussian(4 * normalizedTime);
                    float gaussianValue = gaussianFactor * Values[j];
                    if(j == rangeStart)
                    {
                        maxInRange = gaussianValue;
                        continue;
                    }

                    if (gaussianValue > maxInRange)
                        maxInRange = gaussianValue;
                }

                newValues[currValueIndex] = Values[currValueIndex] / maxInRange;
            }

            Values = newValues;
        }
    }
}
