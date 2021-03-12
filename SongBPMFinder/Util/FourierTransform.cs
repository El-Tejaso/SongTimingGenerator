using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace SongBPMFinder.Util
{
    //This class is untested. further testing is requried before use
    class FourierTransform
    {

        public static void Forward(Slice<float> timeseries, Slice<float> phases, Slice<float> amplitudes, int startingFrequency, int step)
        {
            DFT(timeseries, phases, amplitudes, startingFrequency, step, false);
        }

        public static void Inverse(Slice<float> timeseries, Slice<float> phases, Slice<float> amplitudes, int startingFrequency, int step)
        {
            DFT(timeseries, phases, amplitudes, startingFrequency, step, true);
        }

        /// <summary>
        /// Performs a fourier transform without allocating any memory.
        /// The length of the phases and amplitudes buffers must be the same
        /// The number of steps taken will be determined by the length of the phase/amp buffer
        /// </summary>
        /// <param name="timeseries"></param>
        /// <param name="phases"></param>
        /// <param name="amplitudes"></param>
        /// <param name="scale"></param>
        private static void DFT(Slice<float> timeseries, Slice<float> phases, Slice<float> amplitudes, int startingFrequency, int step, bool scale)
        {
            for (int i = 0; i < phases.Length; i++)
            {
                int k = startingFrequency + i * step;
                float sumR = 0;
                float sumi = 0;
                for (int t = 0; t < timeseries.Length; t++)
                {
                    float xn = timeseries[t];
                    //evaluate the trig functions with doubles for added precision. might be slower tho
                    sumR += xn * (float)(Math.Cos(2.0 * Math.PI * (double)k * t / (double)timeseries.Length));
                    sumi += xn * (float)(Math.Sin(2.0 * Math.PI * (double)k * t / (double)timeseries.Length));
                }

                amplitudes[i] = (float)Math.Sqrt(sumR * sumR + sumi * sumi);
                if (scale)
                    amplitudes[i] /= (float)timeseries.Length;
                phases[i] = (float)Math.Atan2(sumi, sumR);
            }
        }

        
    }
}
