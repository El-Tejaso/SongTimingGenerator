namespace SongBPMFinder
{
    public struct TimeSeriesPeak

    {
        public double Time;
        public int IndexIntoTimeSeries;
        public float PeakHeight;

        public TimeSeriesPeak(double time, float weight, int indexIntoTimeSeries = -1)
        {
            Time = time;
            IndexIntoTimeSeries = indexIntoTimeSeries;
            PeakHeight = weight;
        }
    }
}
