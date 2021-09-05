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
            for (int i = 1; i < Times.Count; i++)
            {
                if (Times[i - 1] >= Times[i])
                {
                    throw new Exception("List isnt sorted bruh");
                }
            }


            return new TimeSeries(Times.ToArray(), Values.ToArray());
        }
    }


    public class TimeSeries
    {
        private double[] times;
        private float[] values;

        public Color Color;
        public int Width = 1;

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
            SpanFunctional.Map(Values, Values, Values, (float a, float b) => {
                if (float.IsNaN(a) || float.IsInfinity(a))
                    return 0;
                return a;
            });
        }


        /// <summary>
        /// Cheers:
        /// 
        /// Brakel, J.P.G. van (2014). 
        /// "Robust peak detection algorithm using z-scores". 
        /// Stack Overflow. Available at: 
        /// https://stackoverflow.com/questions/22583391/peak-signal-detection-in-realtime-timeseries-data/22640362#22640362 
        /// (version: 2020-11-08).
        /// 
        /// Apparently this SO answer was referenced by a bunch of legit research papers.
        /// Not sure if what I programmed is what they referred to, but I think it is.
        /// 
        /// Also it doesnt work that well when windowSize is large, or Im just not using it right.
        /// Its ok though, I have an idea for something similar that might be better
        /// </summary>
        public TimeSeries PeakDetectTimeSeries(double windowSize, float influence, float threshold, bool binary = true)
        {
            TimeSeriesBuilder signalTimeSeries = new TimeSeriesBuilder();

            int rangeStart = 0, rangeEnd = 0;

            while (rangeEnd + 1 < Times.Length && Times[rangeEnd + 1] - Times[rangeStart] < windowSize)
            {
                signalTimeSeries.Add(Times[rangeEnd], 0);
                rangeEnd++;
            }


            Span<float> range = new Span<float>(Values, 0, rangeEnd);

            float mean = MathUtilSpanF.Mean(range, SpanFunctional.None);
            //float mean = 0;
            float standardDev = MathUtilSpanF.StandardDeviation(range);

            float deltaTime = (float)(Times[1] - Times[0]);
            influence *= deltaTime;

            for (; rangeEnd < Times.Length; rangeStart++, rangeEnd++)
            {
                range = new Span<float>(Values, rangeStart, rangeEnd - rangeStart + 1);

                //Possibly inneficient, but easy to program it like this
                float newMean = MathUtilSpanF.Mean(range, SpanFunctional.None);
                float newStandardDev = MathUtilSpanF.StandardDeviation(range);

                mean = MathUtilF.Lerp(mean, newMean, influence);
                standardDev = MathUtilF.Lerp(standardDev, newStandardDev, influence);

                float value = 0;
                if(standardDev > 0.01f)
                {
                    value = (Math.Abs(Values[rangeEnd] - mean) / standardDev) / threshold;
                }


                if (binary)
                {
                    signalTimeSeries.Add(Times[rangeEnd], value > 1 ? 1 : 0);
                }
                else
                {
                    signalTimeSeries.Add(Times[rangeEnd], value);
                }
            }

            return signalTimeSeries.ToTimeSeries();
        }


        /// <summary>
        /// Only works if Times are uniformly spaced
        /// </summary>
        public void MovingAverage(int samples)
        {
            double deltaTime = Times[1] - Times[0];
            double windowSizeSeconds = deltaTime * samples;
            MathUtilSpanF.MovingAverage(Values, Values, samples);

            for (int i = 0; i < Times.Length; i++)
            {
                Times[i] += windowSizeSeconds / 2;
            }
        }


        /// <summary>
        /// My own peak detection algorithm that works best on on smooth data with lots of datapoints.
        /// 
        /// ONLY WORKS IF THE TIMES ARE ALL EVENLY SPACED
        /// </summary>
        public TimeSeries PeakDetectContinuousTimeSeries(double windowSizeSeconds,
            double upwardsGradientThreshold, double downwardsGradientThreshold, double inflectionTime)
        {
            double[] newTimes = new double[Times.Length];
            Array.Copy(Times, 0, newTimes, 0, newTimes.Length);
            float[] newValues = new float[Values.Length];

            float deltaTime = (float)(Times[1] - Times[0]);
            int windowSize = (int)((float)windowSizeSeconds / deltaTime);



            foreach (TimeSeriesPeak peaks in PeakDetectContinuous.DetectPeaks(
                Values, 
                deltaTime, 
                upwardsGradientThreshold, 
                downwardsGradientThreshold, 
                inflectionTime
                ))
            {
                int i = peaks.IndexIntoTimeSeries;
                int rangeStart = Math.Max(i - windowSize/2, 0);
                int rangeEnd = Math.Min(i + windowSize / 2, Times.Length);

                if (rangeEnd - rangeStart < windowSize / 2)
                    continue;


                newValues[i] = 1;//peaks.PeakHeight;
            }

            return new TimeSeries(newTimes, newValues);
        }

    }
}
