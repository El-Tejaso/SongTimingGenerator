using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SongBPMFinder
{
    public static class TimingUtil
    {
        public static TimingPointList GetTiming(AudioData data, IBeatDetector beatDetector, ITimingGenerator timingGenerator)
        {
            AudioSlice mergedChannel = new AudioSlice(new Slice<float>(new float[data[0].Length]), data.SampleRate);

            for(int i = 0; i < data.Length; i++)
            {
                float sample = 0;
                for(int channel = 0; channel < data.NumChannels; channel++)
                {
                    sample += data[channel][i];
                }

                mergedChannel[i] = sample;
            }

            return GetTiming(mergedChannel, beatDetector, timingGenerator);
        }

        public static TimingPointList GetTiming(AudioSlice mergedChannel, IBeatDetector beatDetector, ITimingGenerator timingGenerator)
        {
            SortedList<Beat> beats = beatDetector.GetEveryBeat(mergedChannel);

            TimingPointList timing = timingGenerator.GenerateTiming(beats);

            return timing;
        }

        public static string GetTimingOsuString(AudioData data, IBeatDetector beatDetector, ITimingGenerator timingGenerator)
        {
            return new OsuTimingPointFormatter().FormatTiming(GetTiming(data, beatDetector, timingGenerator));
        }
    }
}
