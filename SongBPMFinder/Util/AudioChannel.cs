using System;

namespace SongBPMFinder
{
    public struct AudioChannel
    {
        private Memory<float> data;
        public int SampleRate;

        public float this[int index] {
            get {
                return data.Span[index];
            }
            set {
                data.Span[index] = value;
            }
        }

        public int Length { get { return data.Length; } }

        public AudioChannel GetSlice(int start, int end)
        {
            if (start < 0)
                start = 0;
            if (end >= data.Length)
                end = data.Length - 1;

            return new AudioChannel(data.Slice(start, end - start), SampleRate);
        }

        public Span<float> GetFloatSlice(int start, int end)
        {
            return data.Span.Slice(start, end - start);
        }

        public double SamplesToSeconds(int timeInSamples)
        {
            return timeInSamples / (double)SampleRate;
        }

        public int SecondsToSamples(double timeInSeconds)
        {
            return (int)(SampleRate * timeInSeconds);
        }

        public AudioChannel(Memory<float> data, int sampleRate)
        {
            this.data = data;
            SampleRate = sampleRate;
        }

        public AudioChannel DeepCopy()
        {
            float[] dataCopy = new float[data.Length];
            data.Span.CopyTo(dataCopy);

            return new AudioChannel(dataCopy, SampleRate);
        }
    }
}
