// Accord Math Library
// The Accord.NET Framework
// http://accord-framework.net
//
// Copyright © Diego Catalano, 2014
// diego.catalano at live.com
//
// Copyright © Nayuki Minase, 2014
// nayuki at eigenstate.org
// http://nayuki.eigenstate.org/page/free-small-fft-in-multiple-languages
//
// Copyright © Guney Ozsan, 2019
// guneyozsan at gmail.com
//
//    This library is free software; you can redistribute it and/or
//    modify it under the terms of the GNU Lesser General Public
//    License as published by the Free Software Foundation; either
//    version 2.1 of the License, or (at your option) any later version.
//
//    This library is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
//    Lesser General Public License for more details.
//
//    You should have received a copy of the GNU Lesser General Public
//    License along with this library; if not, write to the Free Software
//    Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA  02110-1301  USA
//
// Contains code distributed by Project Nayuki, available under a MIT license 
// at http://nayuki.eigenstate.org/page/free-small-fft-in-multiple-languages
//
// The original license is listed below:
//
//    Permission is hereby granted, free of charge, to any person obtaining a copy of
//    this software and associated documentation files (the "Software"), to deal in
//    the Software without restriction, including without limitation the rights to
//    use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of
//    the Software, and to permit persons to whom the Software is furnished to do so,
//    subject to the following conditions:
//
//     - The above copyright notice and this permission notice shall be included in
//       all copies or substantial portions of the Software.
//
//   The Software is provided "as is", without warranty of any kind, express or
//   implied, including but not limited to the warranties of merchantability,
//   fitness for a particular purpose and noninfringement. In no event shall the
//   authors or copyright holders be liable for any claim, damages or other
//   liability, whether in an action of contract, tort or otherwise, arising from,
//   out of or in connection with the Software or the use or other dealings in the
//   Software.
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SongBPMFinder.Slices;

namespace SongBPMFinder.SignalProcessing
{
    /// <summary>
    ///   Fourier Transform (for arbitrary size matrices).
    /// </summary>

    class AccordFourierTransform : IFourierTransform
    {
        public enum Direction
        {
            Forward,
            Backward
        };

        // Trigonometric tables cached.
        private float[] cosTable;
        private float[] sinTable;
        private float[] expCosTable;
        private float[] expSinTable;

        /// <summary>
        ///   1-D Fast Fourier Transform.
        /// </summary>
        /// 
        /// <param name="real">The real part of the complex numbers to transform.</param>
        /// <param name="imag">The imaginary part of the complex numbers to transform.</param>
        /// <param name="direction">The transformation direction.</param>
        /// 
        private void FFT(Slice<float> real, Slice<float> imag, AccordFourierTransform.Direction direction = Direction.Forward)
        {
            if (direction == AccordFourierTransform.Direction.Forward)
            {
                FFT(real, imag);
            }
            else
            {
                FFT(imag, real);
            }

            if (direction == AccordFourierTransform.Direction.Backward)
            {
                for (int i = 0; i < real.Length; i++)
                {
                    real[i] /= real.Length;
                    imag[i] /= real.Length;
                }
            }
        }

        public void Forward(Slice<float> real, Slice<float> imag)
        {
            FFT(real, imag, Direction.Forward);
        }

        public void Backward(Slice<float> real, Slice<float> imag)
        {
            FFT(real, imag, Direction.Backward);
        }

        /// <summary>
        ///   Computes the discrete Fourier transform (DFT) of the given complex vector, 
        ///   storing the result back into the vector. The vector can have any length. 
        ///   This is a wrapper function.
        /// </summary>
        /// 
        /// <param name="real">The real.</param>
        /// <param name="imag">The imag.</param>
        /// 
        private void FFT(Slice<float> real, Slice<float> imag)
        {
            int n = real.Length;

            if (n == 0)
                return;

            if ((n & (n - 1)) == 0)
            {
                // Is power of 2
                TransformRadix2(real, imag);
            }
            else
            {
                // More complicated algorithm for arbitrary sizes
                TransformBluestein(real, imag);
            }
        }


        /// <summary>
        ///   Computes the inverse discrete Fourier transform (IDFT) of the given complex 
        ///   vector, storing the result back into the vector. The vector can have any length.
        ///   This is a wrapper function. This transform does not perform scaling, so the 
        ///   inverse is not a true inverse.
        /// </summary>
        /// 
        private void IDFT(Slice<float> real, Slice<float> imag)
        {
            FFT(imag, real);
        }

        /// <summary>
        ///   Computes the discrete Fourier transform (DFT) of the given complex vector, storing 
        ///   the result back into the vector. The vector's length must be a power of 2. Uses the 
        ///   Cooley-Tukey decimation-in-time radix-2 algorithm.
        /// </summary>
        /// 
        /// <exception cref="System.ArgumentException">Length is not a power of 2.</exception>
        /// 
        private void TransformRadix2(Slice<float> real, Slice<float> imag)
        {
            int n = real.Length;

            int levels = (int)Math.Floor(Math.Log(n, 2));

            if (1 << levels != n)
                throw new ArgumentException("Length is not a power of 2");

            // Trigonometric tables.
            float[] cosTable = CosTable(n / 2);
            float[] sinTable = SinTable(n / 2);

            // Bit-reversed addressing permutation
            for (int i = 0; i < real.Length; i++)
            {
                //int j = unchecked((int)((uint)Reverse(i) >> (32 - levels)));
                int j = unchecked((int)((uint)Reverse(i) >> (32 - levels)));

                if (j > i)
                {
                    var temp = real[i];
                    real[i] = real[j];
                    real[j] = temp;

                    temp = imag[i];
                    imag[i] = imag[j];
                    imag[j] = temp;
                }
            }

            // Cooley-Tukey decimation-in-time radix-2 FFT
            for (int size = 2; size <= n; size *= 2)
            {
                int halfsize = size / 2;
                int tablestep = n / size;

                for (int i = 0; i < n; i += size)
                {
                    for (int j = i, k = 0; j < i + halfsize; j++, k += tablestep)
                    {
                        int h = j + halfsize;
                        float re = real[h];
                        float im = imag[h];

                        float tpre = +re * cosTable[k] + im * sinTable[k];
                        float tpim = -re * sinTable[k] + im * cosTable[k];

                        real[h] = real[j] - tpre;
                        imag[h] = imag[j] - tpim;

                        real[j] += tpre;
                        imag[j] += tpim;
                    }
                }

                // Prevent overflow in 'size *= 2'
                if (size == n)
                    break;
            }
        }


        /// <summary>
        ///   Computes the discrete Fourier transform (DFT) of the given complex vector, storing 
        ///   the result back into the vector. The vector can have any length. This requires the 
        ///   convolution function, which in turn requires the radix-2 FFT function. Uses 
        ///   Bluestein's chirp z-transform algorithm.
        /// </summary>
        /// 
        private void TransformBluestein(Slice<float> real, Slice<float> imag)
        {
            int n = real.Length;
            int m = HighestOneBit(n * 2 + 1) << 1;

            // Trigonometric tables.
            float[] cosTable = ExpCosTable(n);
            float[] sinTable = ExpSinTable(n);

            // Temporary vectors and preprocessing
            var areal = new Slice<float>(new float[m]);
            var aimag = new Slice<float>(new float[m]);
            for (int i = 0; i < real.Length; i++)
            {
                areal[i] = +real[i] * cosTable[i] + imag[i] * sinTable[i];
                aimag[i] = -real[i] * sinTable[i] + imag[i] * cosTable[i];
            }

            var breal = new Slice<float>(new float[m]);
            var bimag = new Slice<float>(new float[m]);
            breal[0] = cosTable[0];
            bimag[0] = sinTable[0];

            for (int i = 1; i < cosTable.Length; i++)
            {
                breal[i] = breal[m - i] = cosTable[i];
                bimag[i] = bimag[m - i] = sinTable[i];
            }

            // Convolution
            var creal = new Slice<float>(new float[m]);
            var cimag = new Slice<float>(new float[m]);
            Convolve(areal, aimag, breal, bimag, creal, cimag);

            // Postprocessing
            for (int i = 0; i < n; i++)
            {
                real[i] = +creal[i] * cosTable[i] + cimag[i] * sinTable[i];
                imag[i] = -creal[i] * sinTable[i] + cimag[i] * cosTable[i];
            }
        }


        /// <summary>
        ///   Computes the circular convolution of the given real 
        ///   vectors. All vectors must have the same length.
        /// </summary>
        /// 
        public void Convolve(Slice<float> x, Slice<float> y, Slice<float> result)
        {
            int n = x.Length;
            Convolve(x, new Slice<float>(new float[n]), y, new Slice<float>(new float[n]), result, new Slice<float>(new float[n]));
        }

        /// <summary>
        ///   Computes the circular convolution of the given complex 
        ///   vectors. All vectors must have the same length.
        /// </summary>
        /// 
        public void Convolve(Slice<float> xreal, Slice<float> ximag, Slice<float> yreal, Slice<float> yimag, Slice<float> outreal, Slice<float> outimag)
        {
            int n = xreal.Length;

            FFT(xreal, ximag);
            FFT(yreal, yimag);

            for (int i = 0; i < xreal.Length; i++)
            {
                var temp = xreal[i] * yreal[i] - ximag[i] * yimag[i];
                ximag[i] = ximag[i] * yreal[i] + xreal[i] * yimag[i];
                xreal[i] = temp;
            }

            IDFT(xreal, ximag);

            // Scaling (because this FFT implementation omits it)
            for (int i = 0; i < n; i++)
            {
                outreal[i] = xreal[i] / n;
                outimag[i] = ximag[i] / n;
            }
        }

        private int HighestOneBit(int i)
        {
            i |= (i >> 1);
            i |= (i >> 2);
            i |= (i >> 4);
            i |= (i >> 8);
            i |= (i >> 16);
            return i - (int)((uint)i >> 1);
        }

        private int Reverse(int i)
        {
            i = (i & 0x55555555) << 1 | (int)((uint)i >> 1) & 0x55555555;
            i = (i & 0x33333333) << 2 | (int)((uint)i >> 2) & 0x33333333;
            i = (i & 0x0f0f0f0f) << 4 | (int)((uint)i >> 4) & 0x0f0f0f0f;
            i = (i << 24) | ((i & 0xff00) << 8) |
                ((int)((uint)i >> 8) & 0xff00) | (int)((uint)i >> 24);
            return i;
        }

        private int ReverseOld(int si)
        {
            uint i = (uint)si;

            i = (i & 0x55555555) << 1 | (i >> 1) & 0x55555555;
            i = (i & 0x33333333) << 2 | (i >> 2) & 0x33333333;
            i = (i & 0x0f0f0f0f) << 4 | (i >> 4) & 0x0f0f0f0f;
            i = (i << 24) | ((i & 0xff00) << 8) |
                ((i >> 8) & 0xff00) | (i >> 24);
            return (int)i;
        }

        /// <summary>
        ///   Creates an evenly spaced frequency vector (assuming a symmetric FFT)
        /// </summary>
        /// 
        public Slice<float> GetFrequencyVector(int length, int sampleRate)
        {
            int numUniquePts = (int)System.Math.Ceiling((length + 1) / 2.0);

            Slice<float> freq = new Slice<float>(new float[numUniquePts]);
            for (int i = 0; i < numUniquePts; i++)
                freq[i] = i * sampleRate / (float)length;

            return freq;
        }

        /// <summary>
        ///   Gets the spectral resolution for a signal of given sampling rate and number of samples.
        /// </summary>
        /// 
        public float GetSpectralResolution(int samplingRate, int samples)
        {
            return samplingRate / (float)samples;
        }


        /// <summary>
        ///   Gets a half period cosine table.
        ///   Keeps the results in memory and reuses if parameters are the same.
        /// </summary>
        /// 
        private float[] CosTable(int sampleCount)
        {
            // Return table from memory if period matches.
            if (cosTable != null && sampleCount == cosTable.Length)
                return cosTable;

            // Create a new table and keep in memory.
            cosTable = new float[sampleCount];
            for (int i = 0; i < sampleCount; i++)
            {
                cosTable[i] = (float)Math.Cos(Math.PI * i / sampleCount);
            }
            return cosTable;
        }

        /// <summary>
        ///   Gets a half period sinus table.
        ///   Keeps the results in memory and reuses if parameters are the same.
        /// </summary>
        ///
        private float[] SinTable(int sampleCount)
        {
            // Return table from memory if period matches.
            if (sinTable != null && sampleCount == sinTable.Length)
                return sinTable;

            // Create a new table and keep in memory.
            sinTable = new float[sampleCount];
            for (int i = 0; i < sampleCount; i++)
            {
                sinTable[i] = (float)Math.Sin(Math.PI * i / sampleCount);
            }
            return sinTable;
        }

        /// <summary>
        ///   Gets a cosine table with exponentially increasing frequency by i * i.
        ///   Keeps the results in memory and reuses if parameters are the same.
        /// </summary>
        ///
        private float[] ExpCosTable(int sampleCount)
        {
            // Return table from memory if period matches.
            if (expCosTable != null && sampleCount == expCosTable.Length)
                return expCosTable;

            // Create a new table and keep in memory.
            expCosTable = new float[sampleCount];
            for (int i = 0; i < sampleCount; i++)
            {
                int j = (int)((long)i * i % (sampleCount * 2));  // This is more accurate than j = i * i
                expCosTable[i] = (float)Math.Cos(Math.PI * j / sampleCount);
            }
            return expCosTable;
        }

        /// <summary>
        ///   Gets a sinus table with exponentially increasing frequency by i * i.
        ///   Keeps the results in memory and reuses if parameters are the same.
        /// </summary>
        ///
        private float[] ExpSinTable(int sampleCount)
        {
            // Return table from memory if period matches.
            if (expSinTable != null && sampleCount == expSinTable.Length)
                return expSinTable;

            // Create a new table and keep in memory.
            expSinTable = new float[sampleCount];
            for (int i = 0; i < sampleCount; i++)
            {
                int j = (int)((long)i * i % (sampleCount * 2));  // This is more accurate than j = i * i
                expSinTable[i] = (float)Math.Sin(Math.PI * j / sampleCount);
            }
            return expSinTable;
        }
    }
}
