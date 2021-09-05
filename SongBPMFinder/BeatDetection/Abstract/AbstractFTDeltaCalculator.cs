using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SongBPMFinder
{
    public abstract class AbstractFTDeltaCalculator
    {
        int minimumFrequency, maximumFrequency;
        protected bool correctFrequencies;

        /// <summary>
        /// minimum inclusive, maximum exclusive
        /// </summary>
        protected AbstractFTDeltaCalculator(int minimumFrequency, int maximumFrequency, bool correctFrequencies)
        {
            this.minimumFrequency = minimumFrequency;
            this.maximumFrequency = maximumFrequency;
            this.correctFrequencies = correctFrequencies;

            if (!(minimumFrequency < 0 && maximumFrequency < 0))
            {
                if (minimumFrequency >= maximumFrequency)
                {
                    throw new ArgumentException("minimumFrequency must be less than maximumFrequency");
                }
            }
        }

        public float Delta(Span<float> lastFourierTransform, Span<float> thisFourierTransform)
        {
            if (maximumFrequency > lastFourierTransform.Length)
            {
                throw new Exception("This delta calculator is not compatible with the fourier transform deltas given");
            }

            int lowerFrequency = minimumFrequency < 0 ? 0 : minimumFrequency;
            int upperFrequency = maximumFrequency < 0 ? lastFourierTransform.Length : maximumFrequency;
            int totalFrequencies = lastFourierTransform.Length;

            float acc = 0;
            for (int currentFreqIndex = lowerFrequency; currentFreqIndex < upperFrequency; currentFreqIndex++)
            {
                float a = lastFourierTransform[currentFreqIndex];
                float b = thisFourierTransform[currentFreqIndex];
                if (correctFrequencies)
                {
                    a = a / (float)(currentFreqIndex + 1);
                    b = b / (float)(currentFreqIndex + 1);
                }

                acc = this.deltaInternal(a, b, acc, currentFreqIndex+1);
            }

            return acc;
        }

        protected abstract float deltaInternal(float a, float b, float accumulator, int frequency);
    }
}
