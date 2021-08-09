namespace SongBPMFinder
{
    public interface IBeatDetector
    {
        SortedList<Beat> GetEveryBeat(AudioSlice audioSlice);
    }
}
