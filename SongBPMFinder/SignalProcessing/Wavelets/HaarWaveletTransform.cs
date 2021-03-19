using SongBPMFinder.Slices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SongBPMFinder.SignalProcessing.Wavelets
{
    public class HaarWaveletTransform : IWaveletTransform
    {
        //Not sure how to remove this copy paste
        static IWaveletTransform _singletonInstace;

        public static IWaveletTransform Implementation {
            get {
                return _singletonInstace;
            }
        }

        static HaarWaveletTransform()
        {
            _singletonInstace = new HaarWaveletTransform();
        }

        // This implementation was taken from the Accord.Net framework and adapted to use floats as well as allocate less memory
        // https://github.com/accord-net/framework/blob/development/Sources/Accord.Math/Wavelets/Haar.cs
        // The temp buffer is manually specified to reduce memory allocations
        public void Transform(Slice<float> data, Slice<float> tempBuffer)
        {
            float w0 = 0.5f;
            float w1 = -0.5f;
            float s0 = 0.5f;
            float s1 = 0.5f;

            int h = data.Length / 2;

            for (int i = 0; i < h; i++)
            {
                int k = i * 2;
                float dK = data[k];
                float dK1 = data[k + 1];
                tempBuffer[i] = dK * s0 + dK1 * s1;
                tempBuffer[i + h] = dK * w0 + dK1 * w1;
            }
        }
    }
}
