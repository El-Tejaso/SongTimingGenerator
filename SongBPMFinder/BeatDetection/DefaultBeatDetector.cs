using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SongBPMFinder
{
    public class DefaultBeatDetector : BeatDetector
    {
        protected override SortedList<Beat> GetEveryBeat(AudioChannel audioSlice)
        {
            SortedList<Beat> beats = new SortedList<Beat>();

            
            //FFT stuff


            return beats;
        }

        float[] FourierTransformHistogram(AudioChannel slice, double position, float timeWindow)
        {
            int start = slice.ToSamples(position);
            int histogramSize = slice.ToSamples(timeWindow);
            Span<float> timePortion = slice.GetSlice(start, start + histogramSize);

            float[] magnitudes = new float[timePortion.Length];

            FourierTransform.FourierTransformMagnitudes(timePortion, magnitudes);

            return magnitudes;
        }
    }
}
