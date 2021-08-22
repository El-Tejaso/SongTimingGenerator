using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SongBPMFinder
{
    public enum FourierDifferenceType
    {
        SumSquares,
        MaxSample,
    }

    public class DefaultBeatDetector : AbstractBeatDetector
    {
        public int FourierWindow = 1024;
        public double EvalDistance = 0.25;
        public double Stride = 0.0005;

        public int FrequencyBands = 4;
        public bool BinaryPeaks = false;

        public bool AddSeperateBands = false;

        public double Start = -1;
        public double End = -1;

        public FourierDifferenceType DifferenceFunction;

        public DefaultBeatDetector(List<TimeSeries> debugTimeSeries)
            : base(debugTimeSeries)
        {
        }

        protected override SortedList<Beat> GetEveryBeat(AudioChannel audioSlice)
        {
            SortedList<Beat> beats = new SortedList<Beat>();

            AudioChannel subsetOfData = getWorkingSet(audioSlice);

            TimeSeries[] fourierDerivatives = calculateFourierDerivatives(subsetOfData);

            if (AddSeperateBands)
            {
                fourierDerivatives = combineTimeSerieses(fourierDerivatives);
            }

            TimeSeries[] peakDetectionSignals = runPeakDetection(fourierDerivatives);

            return beats;
        }

        private TimeSeries[] runPeakDetection(TimeSeries[] fourierDerivatives)
        {
            TimeSeries[] peakDetectionSignals = new TimeSeries[fourierDerivatives.Length];

            for (int i = 0; i < fourierDerivatives.Length; i++)
            {
                fourierDerivatives[i].MovingAverage(45);
                fourierDerivatives[i].Normalize();
                addTimeSeries(fourierDerivatives[i], Color.Yellow);

                TimeSeries peakDetect = fourierDerivatives[i].PeakDetectTimeSeries(0.2, 1, 3.5f, BinaryPeaks);

                peakDetectionSignals[i] = peakDetect;
                
                addTimeSeries(peakDetect, Color.Red, false);
            }

            return peakDetectionSignals;
        }

        private static TimeSeries[] combineTimeSerieses(TimeSeries[] fourierDerivatives)
        {
            for (int i = 1; i < fourierDerivatives.Length; i++)
            {
                MathUtilSpanF.Add(fourierDerivatives[0].Values, fourierDerivatives[i].Values, fourierDerivatives[0].Values);
            }

            fourierDerivatives = new TimeSeries[]
            {
                    fourierDerivatives[0]
            };
            return fourierDerivatives;
        }

        private TimeSeries[] calculateFourierDerivatives(AudioChannel subsetOfData)
        {
            int evalDistanceInSamples = subsetOfData.SecondsToSamples(EvalDistance);

            TimeSeries[] fourierDerivatives = differentiateWithVariedFrequencies(
                            subsetOfData,
                            FourierWindow,
                            evalDistanceInSamples,
                            Color.Yellow,
                            FrequencyBands
                        );

            if(Start >= 0)
            {
                foreach(TimeSeries ts in fourierDerivatives)
                {
                    for(int i = 0; i < ts.Times.Length; i++)
                    {
                        ts.Times[i] += Start;
                    }
                }
            }

            return fourierDerivatives;
        }

        private AudioChannel getWorkingSet(AudioChannel audioSlice)
        {
            int rangeStart = 0;
            int rangeEnd = audioSlice.Length;

            if (Start > 0)
            {
                rangeStart = audioSlice.SecondsToSamples(Start);
                rangeEnd = audioSlice.SecondsToSamples(End);
            }

            AudioChannel subsetOfData = audioSlice.GetSlice(rangeStart, rangeEnd);
            return subsetOfData;
        }

        public AbstractFTDeltaCalculator createDeltaCalculator(int minFrequ, int maxFrequ)
        {
            switch (DifferenceFunction)
            {
                case FourierDifferenceType.SumSquares:
                    return new SumSquareDifferencesFTDeltaCalculator(minFrequ, maxFrequ);
                case FourierDifferenceType.MaxSample:
                    return new MaxDifferenceFTDeltaCalculator(minFrequ, maxFrequ);
                default:
#if DEBUG
                    throw new Exception("Bruh you havent handled this type that u created lmao");
#endif
                    return new SumSquareDifferencesFTDeltaCalculator(minFrequ, maxFrequ);
            }
        }

        private TimeSeries[] differentiateWithVariedFrequencies(AudioChannel audio, int sampleWindow, int evalDistance, Color c, int segmentCount)
        {
            int stride = audio.SecondsToSamples(Stride);

            FourierAudioDifferentiator differentiator = new FourierAudioDifferentiator(sampleWindow, evalDistance, stride);

            AbstractFTDeltaCalculator[] bands = new AbstractFTDeltaCalculator[segmentCount];
            TimeSeriesBuilder[] timeSeries = new TimeSeriesBuilder[segmentCount];

            for (int i = 0; i < segmentCount; i++)
            {
                bands[i] = createDeltaCalculator(i * sampleWindow / segmentCount, (i + 1) * sampleWindow / segmentCount);
                timeSeries[i] = new TimeSeriesBuilder();
            }

            foreach (FTDelta delta in differentiator.AllFourierTransforms(audio))
            {
                for (int i = 0; i < segmentCount; i++)
                {
                    float result = bands[i].Delta(delta.LastFT, delta.ThisFT);
                    timeSeries[i].Add(delta.Time, result);
                }
            }


            TimeSeries[] allTS = new TimeSeries[timeSeries.Length];

            for (int i = 0; i < timeSeries.Length; i++)
            {
                allTS[i] = timeSeries[i].ToTimeSeries();
            }

            return allTS;
        }

        private void addTimeSeries(TimeSeries series, Color c, bool normalize = true)
        {
            //Use if we ever end up adaptively normalizing anything
            //double windowSize = audioPlaybackSystem.CurrentAudioFile.SampleToSeconds(sampleWindow);
            //series.AdaptiveNormalize(windowSize);
            if (normalize)
            {
                series.Normalize();
            }

            float variance = MathUtilSpanF.Variance(series.Values);

            series.Color = c;

            debugTimeSeries.Add(series);
        }
    }
}
