namespace SongBPMFinder
{
    public interface ITimingGenerator
    {
        TimingPointList GenerateTiming(SortedList<Beat> beats);
    }
}
