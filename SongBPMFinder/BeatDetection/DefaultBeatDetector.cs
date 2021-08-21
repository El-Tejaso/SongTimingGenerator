using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SongBPMFinder
{
    public class DefaultBeatDetector : AbstractBeatDetector
    {
        protected override SortedList<Beat> GetEveryBeat(AudioChannel audioSlice)
        {
            SortedList<Beat> beats = new SortedList<Beat>();

            
            //FFT stuff


            return beats;
        }

        float[] FourierTransformHistogram(AudioChannel slice, double position, float timeWindow)
        {
            int start = slice.SecondsToSamples(position);
            int histogramSize = slice.SecondsToSamples(timeWindow);
            Span<float> timePortion = slice.GetSlice(start, start + histogramSize);

            float[] magnitudes = new float[timePortion.Length];

            //SignalHistogram.GenerateSignalHistogram(timePortion, magnitudes);

            return magnitudes;
        }
    }
}
