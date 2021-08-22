using System.Collections.Generic;
using System.Drawing;

namespace SongBPMFinder
{
    public abstract class AbstractBeatDetector
    {
        protected List<TimeSeries> debugTimeSeries;

        protected AbstractBeatDetector(List<TimeSeries> debugTimeSeries)
        {
            this.debugTimeSeries = debugTimeSeries;
        }

        protected abstract SortedList<Beat> GetEveryBeat(AudioChannel audioSlice);

        public SortedList<Beat>[] GetEveryBeat(AudioData data)
        {
            SortedList<Beat>[] channelResults = new SortedList<Beat>[data.NumChannels];

            ///*
            for(int channel = 0; channel < data.NumChannels; channel++)
            {
                channelResults[channel] = GetEveryBeat(data[channel]);
            }
            //*/
            //GetEveryBeat(data[0]);

            return channelResults;
        }

    }
}
