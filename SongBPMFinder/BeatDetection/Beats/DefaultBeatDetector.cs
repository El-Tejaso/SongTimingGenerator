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


            int windowSize = 1024;

            //TimeSeries s1 = testDifferentiatorSettings(audioSlice, windowSize, audio.SecondsToSamples(0.005), Color.Pink);
            //TimeSeries s2 = testDifferentiatorSettings(audioSlice, windowSize, audio.SecondsToSamples(0.01), Color.Red);
            //TimeSeries s3 = testDifferentiatorSettings(audioSlice, windowSize, audio.SecondsToSamples(0.015), Color.Yellow);
            //TimeSeries s4 = testDifferentiatorSettings(audioSlice, windowSize, audio.SecondsToSamples(0.02), Color.Aqua);
            //TimeSeries s5 = testDifferentiatorSettings(audioSlice, windowSize, audio.SecondsToSamples(0.025), Color.Green);
            //TimeSeries s6 = testDifferentiatorSettings(audioSlice, windowSize, audio.SecondsToSamples(0.030), Color.Gold);

            TimeSeries[] fourierDerivatives = differentiateWithVariedFrequencies(audioSlice, windowSize, audioSlice.SecondsToSamples(0.015), Color.Yellow, 4);

            for(int i = 0; i < fourierDerivatives.Length; i++)
            {
                addTimeSeries(fourierDerivatives[i], Color.Yellow);
            }

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

        private TimeSeries[] differentiateWithVariedFrequencies(AudioChannel audio, int sampleWindow, int evalDistance, Color c, int segmentCount)
        {
            int stride = audio.SecondsToSamples(0.0005);

            FourierAudioDifferentiator differentiator = new FourierAudioDifferentiator(sampleWindow, evalDistance, stride);

            AbstractFTDeltaCalculator[] bands = new AbstractFTDeltaCalculator[segmentCount];
            TimeSeriesBuilder[] timeSeries = new TimeSeriesBuilder[segmentCount];

            for (int i = 0; i < segmentCount; i++)
            {
                bands[i] = new MaxDifferenceFTDeltaCalculator(i * sampleWindow / segmentCount, (i + 1) * sampleWindow / segmentCount);
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
            AbstractFTDeltaCalculator calc = new MaxDifferenceFTDeltaCalculator(-1, -1);
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


        private void addTimeSeries(TimeSeries series, Color c)
        {
            //Use if we ever end up adaptively normalizing anything
            //double windowSize = audioPlaybackSystem.CurrentAudioFile.SampleToSeconds(sampleWindow);
            //series.AdaptiveNormalize(windowSize);
            series.Normalize();

            float variance = MathUtilSpanF.Variance(series.Values);

            series.Color = c;

            debugTimeSeries.Add(series);
        }
    }
}
