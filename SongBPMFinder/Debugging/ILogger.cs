namespace SongBPMFinder
{
    public interface ILogger
    {
        string Text { get; }
        void Log(string msg);
        void Clear();
    }
}
