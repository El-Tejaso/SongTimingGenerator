using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SongBPMFinder
{
    public class DefaultBeatDetector : AbstractBeatDetector
    {
        public DefaultBeatDetector(List<TimeSeries> debugTimeSeries)
            : base(debugTimeSeries)
        {
        }

        protected override SortedList<Beat> GetEveryBeat(AudioChannel audioSlice)
        {
            SortedList<Beat> beats = new SortedList<Beat>();

            int windowSize = 512;

            //TimeSeries s1 = testDifferentiatorSettings(audioSlice, windowSize, audio.SecondsToSamples(0.005), Color.Pink);
            //TimeSeries s2 = testDifferentiatorSettings(audioSlice, windowSize, audio.SecondsToSamples(0.01), Color.Red);
            //TimeSeries s3 = testDifferentiatorSettings(audioSlice, windowSize, audio.SecondsToSamples(0.015), Color.Yellow);
            //TimeSeries s4 = testDifferentiatorSettings(audioSlice, windowSize, audio.SecondsToSamples(0.02), Color.Aqua);
            //TimeSeries s5 = testDifferentiatorSettings(audioSlice, windowSize, audio.SecondsToSamples(0.025), Color.Green);
            //TimeSeries s6 = testDifferentiatorSettings(audioSlice, windowSize, audio.SecondsToSamples(0.030), Color.Gold);

            TimeSeries[] fourierDerivatives = differentiateWithVariedFrequencies(
                audioSlice, windowSize, audioSlice.SecondsToSamples(0.025), Color.Yellow, 4);

            for(int i = 1; i < fourierDerivatives.Length; i++)
            {
                SpanFunctional.Map<float,float>(fourierDerivatives[0].Values, fourierDerivatives[i].Values, 
                    fourierDerivatives[0].Values, (float a, float b) => {
                        if (float.IsNaN(b) || float.IsInfinity(b) || b==0)
                            return a;

                        return (a + b); 
                    });
            }


            
            fourierDerivatives[0].MovingAverage(45);
            fourierDerivatives[0].MovingAverage(5);
            fourierDerivatives[0].Normalize();
            addTimeSeries(fourierDerivatives[0], Color.Yellow);
            addTimeSeries(fourierDerivatives[0].PeakDetectTimeSeries(0.2, 1, 
                3.5f, 
                false), Color.Red, false);

            //float gradient = 20f;
            //TimeSeries peaks = fourierDerivatives[0].PeakDetectContinuousTimeSeries(0.5, gradient, gradient, 0.01);
            //addTimeSeries(peaks, Color.Red, false);


            //differentiateWithVariedFrequencies(audioSlice, windowSize, audioSlice.SecondsToSamples(0.025), Color.Green, 4);
            //testDifferentiatorSettingsVariedFrequencies(audioSlice, windowSize, audio.SecondsToSamples(0.015), Color.Yellow, 4);
            /*
            TimeSeries envelope = TimeSeries.CalculateEnvelope(s1, s2, s3, s4);
            envelope.Color = Color.Black;
            envelope.Width = 3;
            addTimeSeries(envelope);
            */


            return beats;
        }

        public AbstractFTDeltaCalculator createDeltaCalculator(int minFrequ, int maxFrequ)
        {
            return new MaxDifferenceFTDeltaCalculator(minFrequ, maxFrequ);
            //return new SumSquareDifferencesFTDeltaCalculator(minFrequ, maxFrequ);
        }

        private TimeSeries[] differentiateWithVariedFrequencies(AudioChannel audio, int sampleWindow, int evalDistance, Color c, int segmentCount)
        {
            int stride = audio.SecondsToSamples(0.0005);

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


        private TimeSeries differentiate(AudioChannel audio, int sampleWindow, int evalDistance, Color c)
        {
            int stride = audio.SecondsToSamples(0.001);

            FourierAudioDifferentiator differentiator = new FourierAudioDifferentiator(sampleWindow, evalDistance, stride);
            AbstractFTDeltaCalculator calc = createDeltaCalculator(-1, -1);
            TimeSeriesBuilder tsb = new TimeSeriesBuilder();

            foreach (FTDelta delta in differentiator.AllFourierTransforms(audio))
            {
                float result = calc.Delta(delta.LastFT, delta.ThisFT);
                tsb.Add(delta.Time, result);
            }

            TimeSeries series = tsb.ToTimeSeries();

            addTimeSeries(series, c);

            return series;
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
