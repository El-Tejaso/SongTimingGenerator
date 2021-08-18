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
        public double[] Times;
        public float[] Values;

        public Color Color;
        public int Width = 2;

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

        //internal static TimeSeries CalculateEnvelope(params TimeSeries[] multipleSeries)
        //{
            
        //}
    }
}
