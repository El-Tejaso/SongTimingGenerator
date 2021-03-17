using SongBPMFinder.Slices;
using SongBPMFinder.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SongBPMFinder.SignalProcessing
{
    public static class WaveletTransforms
    {
        // This implementation was taken from the Accord.Net framework and adapted to use floats as well as allocate less memory
        // https://github.com/accord-net/framework/blob/development/Sources/Accord.Math/Wavelets/Haar.cs
        // The temp buffer is manually specified to reduce memory allocations
        static void haarFWTSingle(Slice<float> data, Slice<float> dst)
        {
            float w0 = 0.5f;
            float w1 = -0.5f;
            float s0 = 0.5f;
            float s1 = 0.5f;

            int h = data.Length / 2;

            for (int i = 0; i < h; i++)
            {
                int k = (i * 2);
                float dK = data[k];
                float dK1 = data[k + 1];
                dst[i] = dK * s0 + dK1 * s1;
                dst[i + h] = dK * w0 + dK1 * w1;
            }
        }

        public static Slice<float>[] GetDetailCoefficients(Slice<float>[] dwtSlices)
        {
            Slice<float>[] newSlices = new Slice<float>[dwtSlices.Length];
            Array.Copy(dwtSlices, 0, newSlices, 0, dwtSlices.Length);

            //Resize DWT slices so that they only keep their detail coefficients
            for (int i = 0; i < dwtSlices.Length; i++)
            {
                newSlices[i] = dwtSlices[i].GetSlice(dwtSlices[i].Length / 2, dwtSlices[i].Length);
            }
            return newSlices;
        }

        //Performs a Haar DWT in the data numLevels times, and returns slices corresponding to each segment
        //The slices will index into the data buffer as well as the data array
        public static Slice<float>[] HaarFWT(Slice<float> data, Slice<float> dataBuffer, int numLevels)
        {
            Slice<float>[] outSlices = new Slice<float>[numLevels];

            for (int i = 0, h = data.Length; i < numLevels; i++, h /= 2)
            {
                Slice<float> inSlice = data.GetSlice(0, h);
                outSlices[i] = dataBuffer.GetSlice(0, h);

                haarFWTSingle(inSlice, dataBuffer);

                Slice<float> temp = data;
                data = dataBuffer;
                dataBuffer = temp;
            }

            outSlices = GetDetailCoefficients(outSlices);

            return outSlices;
        }

        public static Slice<float>[] DownsampleCoefficients(Slice<float>[] dwtSlices, Slice<float> original, int mainDownsampleFactor)
        {
            int numLevels = dwtSlices.Length;

            Slice<float>[] downsampleSlices = new Slice<float>[numLevels];
            int sliceLen = (original.Length / mainDownsampleFactor);

            for (int i = 0; i < numLevels; i++)
            {
                downsampleSlices[i] = original.GetSlice(i * sliceLen, (i + 1) * sliceLen);

                //dwtSlice[level] should have a length of original.Length / level
                int downsampleFactor = mainDownsampleFactor / (original.Length / dwtSlices[numLevels - i - 1].Length);

                SliceMathf.DownsampleAverage(dwtSlices[numLevels - i - 1], downsampleSlices[i], downsampleFactor);
            }

            return downsampleSlices;
        }
    }
}
