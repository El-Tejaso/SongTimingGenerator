namespace SongBPMFinder
{
    //intended to represent only a single channel.
    //The main difference between this and Slice[float] is that it has a 
    public struct AudioSlice
    {
        private Slice<float> data;

        public Slice<float> Slice { get => data; }

        public float this[int index] {
            get {
                return data[index];
            }
            set {
                data[index] = value;
            }
        }

        public int Length { get { return data.Length; } }

        //The samples in one second of audio
        public int SampleRate;

        public AudioSlice GetSlice(int newStart, int newEnd, int newStride = 1)
        {
            Slice<float> slice = data.GetSlice(newStart, newEnd, newStride);
            int newSampleRate = SampleRate / newStride;

            return new AudioSlice(slice, newSampleRate);
        }

        public double ToSeconds(int timeInSamples)
        {
            return timeInSamples / (double)SampleRate;
        }

        public int ToSamples(double timeInSeconds)
        {
            return (int)(SampleRate * timeInSeconds);
        }

        public AudioSlice(Slice<float> data, int sampleRate)
        {
            this.data = data;
            SampleRate = sampleRate;
        }

        public AudioSlice DeepCopy()
        {
            return new AudioSlice(data.DeepCopy(), SampleRate);
        }
    }
}
