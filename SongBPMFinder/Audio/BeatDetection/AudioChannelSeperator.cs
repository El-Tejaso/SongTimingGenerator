using SongBPMFinder.Slices;
using SongBPMFinder.SignalProcessing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SongBPMFinder.Audio.BeatDetection
{
    public struct FrequencyBand
    {
        public float lowerHz;
        public float upperHz;
    }
    public static class AudioChannelSeperator
    {
        public static List<FrequencyBand> ProminentFrequencyBands(Slice<float> data, int sampleRate)
        {
            throw new NotImplementedException();
        }

        public static Slice<float> GetSlice(Slice<float> data, int sampleRate, FrequencyBand band)
        {
            throw new NotImplementedException();
        }
    }
}
