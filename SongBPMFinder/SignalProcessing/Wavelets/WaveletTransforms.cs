using SongBPMFinder.Slices;
using SongBPMFinder.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SongBPMFinder.SignalProcessing.Wavelets
{
    public static class WaveletTransforms
    {
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


        public static Slice<float>[] DWT(IWaveletTransform transformImplementation, Slice<float> data, Slice<float> dataBuffer, int numLevels)
        {
            Slice<float>[] outSlices = new Slice<float>[numLevels];

            for (int i = 0, h = data.Length; i < numLevels; i++, h /= 2)
            {
                Slice<float> inSlice = data.GetSlice(0, h);

                outSlices[i] = dataBuffer.GetSlice(0, h);

                transformImplementation.Transform(inSlice, dataBuffer);

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
            int sliceLen = original.Length / mainDownsampleFactor;

            for (int i = 0; i < numLevels; i++)
            {
                downsampleSlices[i] = original.GetSlice(i * sliceLen, (i + 1) * sliceLen);

                //dwtSlice[level] should have a length of original.Length / level
                int downsampleFactor = mainDownsampleFactor / (original.Length / dwtSlices[numLevels - i - 1].Length);

                FloatSlices.DownsampleMax(dwtSlices[numLevels - i - 1], downsampleSlices[i], downsampleFactor);
            }

            return downsampleSlices;
        }
    }
}
