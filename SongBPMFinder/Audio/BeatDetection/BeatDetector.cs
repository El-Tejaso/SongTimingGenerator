using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using SongBPMFinder.Util;
using SongBPMFinder.Audio.Timing;

namespace SongBPMFinder.Audio.BeatDetection
{
    class BeatDetector
    {
        /// <summary>
        /// Finds a beat within the given sample window.
        /// If there are multiple, it returns the 'most powerful'
        /// TBH this is very rng.
        /// I would reccomend doing a deep copy of the slice before passing it in,
        /// as all operations done are destructive
        /// and you may want to preserve other things in the original array (required if using this function multiple times)
        /// </summary>
        /// <param name="audioData">the audio file where the slice originated. mainly used for conversion factors</param>
        /// <param name="slice">a slice of data from the audio file. The length must be divisable by 2^numLevels otherwise this might not work that well</param>
        /// <param name="sliceSizedBuffer">a slice of data the same length as data, allocated outside the function to reduce memory allocations overall. must not overlap with data. </param>
        /// <param name="instant">The time in seconsds considered as the smallest unit. osu! uses 0.001 (aka 1 millisecond), your rhythm game may not. Don't set this too small however</param>
        /// <param name="numLevels">Internal variable relating to the wavelet transform. delete later if not needed</param>
        /// <returns>An integer corresponding to the sample in the slice where the beat occured</returns>
        public static int DetectBeat(AudioData audioData, Slice<float> slice, Slice<float> sliceSizedBuffer, float instant, int numLevels = 4, bool debug = false)
        {
            Slice<float> sliceDebugCopy = slice.DeepCopy();
            Slice<float>[] dwtSlices = DiscreteWaveletTransform(slice, sliceSizedBuffer, numLevels);

            FullWaveRectify(slice);

            //instantSize is a downsampling factor used later to convert from downsampled indices back to audio indices
            int instantSize = CalculateInstantSize(audioData, instant, numLevels);
            Slice<float>[] downsampledSlices = FloatArrays.DownsampleCoefficients(dwtSlices, slice, instantSize);

            Slice<float> summedEnvelopes = SumEnvelopes(downsampledSlices);

            NormalizeEnvelopes(summedEnvelopes);

            if (debug)
                GenerateDebugPlots(sliceDebugCopy, audioData, summedEnvelopes);
            
            int maxIndex = FindPeak(summedEnvelopes);
            float beatStrength = CalculateBeatStrength(summedEnvelopes, maxIndex);

            if (beatStrength < 1.0f)
            {
                //just because this is the max sample, doesn't necesarily mean that it is siginificant in any way
                return -1;
            }

            //convert back to position in audio
            int maxAudioPos = CalculateUpsampledPosition(instantSize, maxIndex);
            return maxAudioPos;
        }

        private static int FindPeak(Slice<float> summedEnvelopes)
        {
            return FloatArrays.ArgMax(summedEnvelopes, false);
        }

        private static float CalculateBeatStrength(Slice<float> summedEnvelopes, int beatPosition)
        {
            float max = summedEnvelopes[beatPosition];
            float av = FloatArrays.Average(summedEnvelopes);
            float stdDev = FloatArrays.StdDev(summedEnvelopes);
            float meanAbs = FloatArrays.Average(summedEnvelopes, true);

            float ratio = (max - av) / meanAbs;

            //TODO: make this not an arbitrary number
            return ratio / 7.0f;
        }

        private static int CalculateUpsampledPosition(int downsampleFactor, int index)
        {
            return index * downsampleFactor;
        }

        private static void GenerateDebugPlots(Slice<float> sliceDebugCopy, AudioData audioData, Slice<float> summedEnvelopes)
        {
            int maxIndex = FindPeak(sliceDebugCopy);

            //Draw autocorrelated array
            Slice<float> plotArray = summedEnvelopes.DeepCopy();

            Form1.Instance.Plot("Autocorrelated", plotArray, 0);
            List<TimingPoint> debugLines = new List<TimingPoint>();
            debugLines.Add(new TimingPoint(maxIndex, audioData.ToSample(maxIndex), Color.Red));
            Form1.Instance.AddLines(debugLines, 0);

            //Draw frequency spectrum
            AccordFourierTransform.FFT(sliceDebugCopy, FloatArrays.ZeroesLike(sliceDebugCopy), AccordFourierTransform.Direction.Forward);
            Form1.Instance.Plot("Frequencies", sliceDebugCopy.GetSlice(0, sliceDebugCopy.Length / 2), 1);
        }

        private static void NormalizeEnvelopes(Slice<float> summedEnvelopes)
        {
            float mean = FloatArrays.Average(summedEnvelopes, false);
            FloatArrays.Sum(summedEnvelopes, -mean);
            FloatArrays.Normalize(summedEnvelopes);
        }

        private static Slice<float> Autocorrelate(Slice<float> summedEnvelopes)
        {
            Slice<float> autocorrelPlacement = summedEnvelopes.GetSlice(summedEnvelopes.Length, 2 * summedEnvelopes.Length);
            FloatArrays.Autocorrelate(summedEnvelopes, autocorrelPlacement);
            return autocorrelPlacement;
        }

        private static Slice<float> SumEnvelopes(Slice<float>[] downsampleSlices)
        {
            Slice<float> summedEnvelopes = downsampleSlices[0];

            for (int i = 1; i < downsampleSlices.Length; i++)
            {
                FloatArrays.Sum(summedEnvelopes, downsampleSlices[i]);
            }

            return summedEnvelopes;
        }

        private static int CalculateInstantSize(AudioData audioData, float instant, int numLevels)
        {
            int instantSize = audioData.ToSample(instant);
            int requiredPowerOf2 = QuickMafs.Pow(2, numLevels);
            instantSize = QuickMafs.NearestDivisor(instantSize, requiredPowerOf2);
            return instantSize;
        }

        private static Slice<float>[] DiscreteWaveletTransform(Slice<float> slice, Slice<float> sliceSizedBuffer, int numLevels)
        {
            return FloatArrays.HaarFWT(slice, sliceSizedBuffer, numLevels);
        }

        private static void FullWaveRectify(Slice<float> slice)
        {
            FloatArrays.Abs(slice);
        }
    }
}
