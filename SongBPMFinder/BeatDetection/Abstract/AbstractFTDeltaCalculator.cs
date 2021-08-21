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

        /// <summary>
        /// minimum inclusive, maximum exclusive
        /// </summary>
        protected AbstractFTDeltaCalculator(int minimumFrequency, int maximumFrequency)
        {
            this.minimumFrequency = minimumFrequency;
            this.maximumFrequency = maximumFrequency;


            if (!(minimumFrequency < 0 && maximumFrequency < 0))
            {
                if (minimumFrequency >= maximumFrequency)
                {
                    throw new ArgumentException("minimumFrequency must be less than maximumFrequency");
                }
            }
        }

        public float Delta(float[] lastFourierTransform, float[] thisFourierTransform)
        {
            if (maximumFrequency > lastFourierTransform.Length)
            {
                throw new Exception("This delta calculator is not compatible with the fourier transform deltas given");
            }

            int lowerFrequency = minimumFrequency < 0 ? 0 : minimumFrequency;
            int upperFrequency = maximumFrequency < 0 ? lastFourierTransform.Length : maximumFrequency;


            float acc = 0;
            for (int j = lowerFrequency; j < upperFrequency; j++)
            {
                acc = this.deltaInternal(lastFourierTransform[j], thisFourierTransform[j], acc);
            }

            return acc;
        }

        protected abstract float deltaInternal(float a, float b, float accumulator);
    }
}
