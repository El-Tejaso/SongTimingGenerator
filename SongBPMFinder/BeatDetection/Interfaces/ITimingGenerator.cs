namespace SongBPMFinder
{
    public interface ITimingGenerator
    {
        TimingPointList GenerateTiming(SortedList<Beat>[] beatsInEachChannel);
    }
}
