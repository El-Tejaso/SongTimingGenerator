using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exocortex.DSP;


namespace SongBPMFinder
{
    /// <summary>
    /// Defines boundary between this and exocortex/other FFT libraries
    /// </summary>
    public static class FourierTransform
    {
        private static float[] _intermediateBuffer = null;

        private static void syncIntermediateBuffer(int requiredLength)
        {
            if (_intermediateBuffer == null)
            {
                _intermediateBuffer = new float[requiredLength];
                return;
            }
            
            if(_intermediateBuffer.Length < requiredLength)
            {
                int newSize = Math.Max(_intermediateBuffer.Length * 2, requiredLength);
                _intermediateBuffer = new float[newSize];
            }
        }


        /// <summary>
        /// input: span of real numbers, no imaginary numbers.
        /// output: float array of real numbers corresponding to magnitudes.
        /// 
        /// Of course, the backwards pass requires complex numbers and cannot discard magnitudes, so 
        /// FFTBackwardsMagnitudes won't ever exist
        /// </summary>
        public static void FFTForwardsMagnitudes(Span<float> input, float[] output)
        {
            copyRealsToIB(input);

            exocortexFFTOnIB();

            copyMagnitudesFromIBToFloatArray(output);
        }

        private static void copyRealsToIB(Span<float> input)
        {
            syncIntermediateBuffer(input.Length * 2);

            for (int i = 0; i < input.Length; i++)
            {
                _intermediateBuffer[i * 2] = input[i];
                _intermediateBuffer[i * 2 + 1] = 0;
            }
        }

        private static void exocortexFFTOnIB()
        {
            Fourier.FFT(_intermediateBuffer, _intermediateBuffer.Length/2, FourierDirection.Forward);
        }

        private static void copyMagnitudesFromIBToFloatArray(float[] output)
        {
            for (int i = 0; i < _intermediateBuffer.Length; i += 2)
            {
                output[i / 2] = MathUtilF.Magnitude(_intermediateBuffer[i], _intermediateBuffer[i + 1]);
            }
        }
    }
}
