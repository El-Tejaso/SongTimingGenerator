using Exocortex.DSP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SongBPMFinder
{
    /// <summary>
    /// Keep this instance around so you can reuse the differentiate function with fewer allocations
    /// </summary>
    public class FourierAudioDifferentiator : IAudioDifferentiator
    {
        public int SampleWindow { get => sampleWindow; set => sampleWindow = value; }
        public int Stride { get => stride; set => stride = value; }

        private int sampleWindow = 1024;
        private int stride = 512;

        float[] lastFT;
        float[] thisFT;

        public FourierAudioDifferentiator(int sampleWindow, int stride)
        {
            this.SampleWindow = sampleWindow;
            this.Stride = stride;

            lastFT = new float[sampleWindow * 2];
            thisFT = new float[sampleWindow * 2];
        }

        //Implements IAudioDifferentiator.Differentiate
        public TimeSeries Differentiate(AudioChannel audioChannel)
        {
            TimeSeriesBuilder series = new TimeSeriesBuilder();

            audioChannel = doFirstFourierTransform(audioChannel);

            for (int i = Stride; i + SampleWindow < audioChannel.Length; i += Stride)
            {
                audioChannel = doNextFourierTransform(audioChannel, i);

                float sumDifference = calculateDifference();

                double t = audioChannel.SamplesToSeconds(i);
                series.Add(t, sumDifference);

                swapFirstNextBuffers();
            }

            return series.ToTimeSeries();
        }

        private AudioChannel doNextFourierTransform(AudioChannel audioChannel, int i)
        {
            Span<float> slice = audioChannel.GetSlice(i, i + SampleWindow);
            FourierTransform.FFTForwardsMagnitudes(slice, thisFT);
            return audioChannel;
        }

        private AudioChannel doFirstFourierTransform(AudioChannel audioChannel)
        {
            Span<float> firstSlice = audioChannel.GetSlice(0, SampleWindow);
            FourierTransform.FFTForwardsMagnitudes(firstSlice, lastFT);
            return audioChannel;
        }

        private float calculateDifference()
        {
            float sumDifference = 0;

            for (int j = 0; j < lastFT.Length; j++)
            {
                float delta = thisFT[j] - lastFT[j];
                sumDifference += delta * delta;
            }

            return sumDifference;
        }

        private void swapFirstNextBuffers()
        {
            float[] temp = thisFT;
            thisFT = lastFT;
            lastFT = temp;
        }
    }
}
