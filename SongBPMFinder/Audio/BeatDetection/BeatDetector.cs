using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using SongBPMFinder.Util;
using SongBPMFinder.Audio.Timing;
using SongBPMFinder.SignalProcessing;
using SongBPMFinder.Slices;
using SongBPMFinder.SignalProcessing.Wavelets;
using SongBPMFinder.Logging;

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
        public static BeatData DetectBeat(AudioData audioData, Slice<float> slice, Slice<float> sliceSizedBuffer, float instant, int numLevels = 4, bool debug = false)
        {
            //TODO: delete in production
            Slice<float> sliceDebugCopy = slice.DeepCopy();

            //instantSize is a downsampling factor used later in the code to convert from downsampled indices back to audio indices
            int instantSize = CalculateInstantSize(audioData, instant, numLevels);

            Slice<float> summedEnvelopes;

            Slice<float>[] dwtSlicesDebug = null;

            if(numLevels > 0)
            {
                Slice<float>[] dwtSlices = DiscreteWaveletTransform(slice, sliceSizedBuffer, numLevels);
                dwtSlicesDebug = dwtSlices;

                FullWaveRectify(slice);

                Slice<float>[] downsampledSlices = DownsampleSlicesToSameLength(slice, dwtSlices, instantSize);

                summedEnvelopes = SumEnvelopes(downsampledSlices);
            }
            else
            {
                FullWaveRectify(slice);

                summedEnvelopes = slice.GetSlice(0, slice.Length / instantSize);

                FloatSlices.DownsampleAverage(slice, summedEnvelopes, instantSize);
            }

            Slice<float> summedEvelopesCopy = summedEnvelopes.DeepCopy();

            EnhancePeak(summedEnvelopes);

            if (debug)
            {
                GenerateDebugPlots(sliceDebugCopy, audioData, summedEnvelopes, instantSize, summedEvelopesCopy, slice, dwtSlicesDebug);
            }

            int maxIndex = FindPeak(summedEnvelopes);

            float beatStrength = CalculateBeatStrength(summedEnvelopes, maxIndex);

            int maxAudioPos = CalculateUpsampledPosition(instantSize, maxIndex);
            return new BeatData(maxAudioPos, beatStrength);
        }

        private static void EnhancePeak(Slice<float> summedEnvelopes)
        {
            FloatSlices.MovingAverageOffset(summedEnvelopes, 5, 7);

            //*
            FloatSlices.Sum(summedEnvelopes, -FloatSlices.Average(summedEnvelopes));

            for (int i = 0; i < 4; i++)
            {
                FloatSlices.Normalize(summedEnvelopes);
                FloatSlices.Max(summedEnvelopes, 0);
            }
            //*/
        }

        private static Slice<float>[] DownsampleSlicesToSameLength(Slice<float> slice, Slice<float>[] dwtSlices, int instantSize)
        {
            return WaveletTransforms.DownsampleCoefficients(dwtSlices, slice, instantSize);
        }

        private static int FindPeak(Slice<float> summedEnvelopes)
        {
            return FloatSlices.ArgMax(summedEnvelopes, false);
        }

        private static float CalculateBeatStrength(Slice<float> summedEnvelopes, int beatPosition)
        {
            float max = summedEnvelopes[beatPosition];
            float av = FloatSlices.Average(summedEnvelopes);
            float stdDev = FloatSlices.StdDev(summedEnvelopes);
            float meanAbs = FloatSlices.Average(summedEnvelopes, true);

            float ratio = (max - av) / meanAbs;

            //TODO: make this not an arbitrary number
            return ratio / 9.0f;
        }

        private static int CalculateUpsampledPosition(int downsampleFactor, int index)
        {
            return index * downsampleFactor;
        }

        private static void GenerateDebugPlots(Slice<float> sliceDebugCopy, AudioData audioData, Slice<float> summedEnvelopes, int instantSize, Slice<float> summedEvelopesCopy, Slice<float> slice, Slice<float>[] dwtSlicesDebug)
        {
            int maxIndex = FindPeak(summedEnvelopes);

            //Draw autocorrelated array
            Slice<float> plotArray = summedEnvelopes.DeepCopy();

            Form1.Instance.Plot("Autocorrelated", plotArray, 0);
            List<TimingPoint> debugLines = new List<TimingPoint>();
            debugLines.Add(new TimingPoint(maxIndex, audioData.SampleToSeconds(maxIndex), Color.Red));
            Form1.Instance.AddLines(debugLines, 0);

            int dt = audioData.SampleRate / instantSize;

            //Draw moving average
            Slice<float> derivative = plotArray.DeepCopy();
            FloatSlices.Differentiate(derivative, dt);
            Form1.Instance.Plot("Acl dx", derivative, 1);

            //orignal
            Form1.Instance.Plot("Acl original", summedEvelopesCopy, 2);

            //Slice
            Form1.Instance.Plot("Slice", slice, 3);
            List<TimingPoint> debugLines2 = new List<TimingPoint>();
            for(int i = 0; i < dwtSlicesDebug.Length; i++)
            {
                debugLines2.Add(new TimingPoint(0, audioData.SampleToSeconds(dwtSlicesDebug[i].InternalStart)));
            }
            Form1.Instance.AddLines(debugLines2, 3);

            //Draw frequency spectrum
            FourierTransform.Forward(sliceDebugCopy, FloatSlices.ZeroesLike(sliceDebugCopy));
            Form1.Instance.Plot("Frequencies", sliceDebugCopy, 4);

        }

        private static void NormalizeEnvelopes(Slice<float> summedEnvelopes)
        {
            float mean = FloatSlices.Average(summedEnvelopes, false);
            FloatSlices.Sum(summedEnvelopes, -mean);
            FloatSlices.Normalize(summedEnvelopes);
        }

        private static Slice<float> Autocorrelate(Slice<float> summedEnvelopes)
        {
            Slice<float> autocorrelPlacement = summedEnvelopes.GetSlice(summedEnvelopes.Length, 2 * summedEnvelopes.Length);
            FloatSlices.Autocorrelate(summedEnvelopes, autocorrelPlacement);
            return autocorrelPlacement;
        }

        private static Slice<float> SumEnvelopes(Slice<float>[] downsampleSlices)
        {
            Slice<float> summedEnvelopes = downsampleSlices[0];

            for (int i = 1; i < downsampleSlices.Length; i++)
            {
                FloatSlices.Sum(summedEnvelopes, downsampleSlices[i]);
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
            return WaveletTransforms.DWT(DB4WaveletTransform.Implementation, slice, sliceSizedBuffer, numLevels);
        }

        private static void FullWaveRectify(Slice<float> slice)
        {
            FloatSlices.Abs(slice);
        }
    }
}
