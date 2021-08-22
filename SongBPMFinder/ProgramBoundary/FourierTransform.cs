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
    /// Defines boundary between this and exocortex/other FFT libraries.
    /// 
    /// Each instance should be threadsafe but having a different instance for each thread
    /// would be way better for performance
    /// 
    /// TODO: Check if Exocortex is threadsafe/needs instancing for threading performance
    /// </summary>
    public class FourierTransform
    {
        private bool _ibLocked = false;
        private float[] _intermediateBuffer = null;
        private int _complexNumbersInBuffer = 0;

        private void getIBLock()
        {
            while (_ibLocked)
            { }
        }

        private void releaseIBLock()
        {
            _ibLocked = false;
        }

        private void syncIntermediateBuffer(int requiredLength)
        {
            getIBLock();

            if (_intermediateBuffer == null)
            {
                _intermediateBuffer = new float[requiredLength];
                return;
            }

            if (_intermediateBuffer.Length < requiredLength)
            {
                int newSize = Math.Max(_intermediateBuffer.Length * 2, requiredLength);
                _intermediateBuffer = new float[newSize];
            }

            releaseIBLock();
        }


        /// <summary>
        /// input: span of real numbers, no imaginary numbers.
        /// output: float array of real numbers corresponding to magnitudes.
        /// 
        /// Of course, the backwards pass requires complex numbers and cannot discard magnitudes, so 
        /// FFTBackwardsMagnitudes won't ever exist
        /// </summary>
        public void FFTForwardsMagnitudes(Span<float> input, float[] output)
        {
            SpanFunctional.AssertEqualLength<float, float>(input, output);

            copyRealsToIB(input);

            exocortexFFTOnIB();

            copyMagnitudesFromIBToFloatArray(output);
        }

        private void copyRealsToIB(Span<float> input)
        {
            syncIntermediateBuffer(input.Length * 2);

            getIBLock();

            for (int i = 0; i < input.Length; i++)
            {
                _intermediateBuffer[i * 2] = input[i];
                _intermediateBuffer[i * 2 + 1] = 0;
            }

            _complexNumbersInBuffer = input.Length;

            releaseIBLock();
        }

        private void exocortexFFTOnIB()
        {
            getIBLock();

            Fourier.FFT(_intermediateBuffer, _complexNumbersInBuffer, FourierDirection.Forward);

            releaseIBLock();
        }

        private void copyMagnitudesFromIBToFloatArray(float[] output)
        {
            getIBLock();

            for (int i = 0; i < _complexNumbersInBuffer; i += 2)
            {
                output[i / 2] = MathUtilF.Magnitude(_intermediateBuffer[i], _intermediateBuffer[i + 1]);
            }

            releaseIBLock();
        }
    }
}
