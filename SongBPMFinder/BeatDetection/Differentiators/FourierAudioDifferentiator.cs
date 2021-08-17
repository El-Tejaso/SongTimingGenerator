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
        public static readonly int FOURIER_WINDOW = 1024;
        float[] fourierTransformData = new float[FOURIER_WINDOW * 2];

        //Implements IAudioDifferentiator.Differentiate
        public TimeSeries Differentiate(AudioChannel audioChannel)
        {
            TimeSeries series = new TimeSeries();
            Random r = new Random();

            int windowSize = FOURIER_WINDOW;
            float[] lastFT = new float[windowSize * 2];
            float[] thisFT = new float[windowSize * 2];


            for (int i = 0; i + windowSize < audioChannel.Length; i += windowSize/3)
            {
                Span<float> slice = audioChannel.GetSlice(i, i + windowSize);

                if (i == 0)
                {
                    FourierTransform.FFTForwardsMagnitudes(slice, lastFT);
                    continue;
                }

                FourierTransform.FFTForwardsMagnitudes(slice, thisFT);

                float sumDifference = 0;

                for (int j = 0; j < lastFT.Length; j++)
                {
                    float delta = thisFT[j] - lastFT[j];
                    sumDifference += delta * delta;
                }

                float[] temp = thisFT;
                thisFT = lastFT;
                lastFT = temp;

                double t = audioChannel.SamplesToSeconds(i);
                series.Add(t, sumDifference);
            }

            return series;
        }
    }
}
