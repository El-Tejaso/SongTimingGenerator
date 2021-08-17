namespace SongBPMFinder
{
    public abstract class BeatDetector
    {
        protected abstract SortedList<Beat> GetEveryBeat(AudioChannel audioSlice);

        public SortedList<Beat>[] GetEveryBeat(AudioData data)
        {
            SortedList<Beat>[] channelResults = new SortedList<Beat>[data.NumChannels];

            for(int channel = 0; channel < data.NumChannels; channel++)
            {
                channelResults[channel] = GetEveryBeat(data[channel]);
            }

            return channelResults;
        }
    }
}
