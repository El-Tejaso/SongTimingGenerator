using System;

namespace SongBPMFinder
{
    public struct AudioChannel
    {
        private float[] data;
        public int SampleRate;

        public float this[int index] {
            get {
                return data[index];
            }
            set {
                data[index] = value;
            }
        }

        public int Length { get { return data.Length; } }

        public Span<float> GetSlice(int start, int end)
        {
            return new Span<float>(data, start, end - start);
        }

        public double SamplesToSeconds(int timeInSamples)
        {
            return timeInSamples / (double)SampleRate;
        }

        public int SecondsToSamples(double timeInSeconds)
        {
            return (int)(SampleRate * timeInSeconds);
        }

        public AudioChannel(float[] data, int sampleRate)
        {
            this.data = data;
            SampleRate = sampleRate;
        }
        public AudioChannel DeepCopy()
        {
            float[] dataCopy = new float[data.Length];
            data.CopyTo(dataCopy, 0);

            return new AudioChannel(dataCopy, SampleRate);
        }
    }
}
