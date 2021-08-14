using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SongBPMFinder
{
    public class DefaultBeatDetector : BeatDetector
    {
        protected override SortedList<Beat> GetEveryBeat(AudioSlice audioSlice)
        {
            SortedList<Beat> beats = new SortedList<Beat>();

            

            return beats;
        }

        Slice<float> FourierTransformHistogram(AudioSlice slice, double position, float timeWindow)
        {
            int start = slice.ToSamples(position);
            int histogramSize = slice.ToSamples(timeWindow);
            Slice<float> timePortion = slice.GetSlice(start, start + histogramSize).Slice;

            Slice<float> magnitudes = SliceFunctional.ZeroesLike(timePortion);

            FourierTransform.FourierTransformMagnitudes(timePortion, magnitudes);

            return magnitudes;
        }
    }
}
