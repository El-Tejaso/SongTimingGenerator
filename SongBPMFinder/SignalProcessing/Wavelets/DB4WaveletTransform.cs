using SongBPMFinder.Slices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SongBPMFinder.SignalProcessing.Wavelets
{
    class DB4WaveletTransform : IWaveletTransform
    {
        static IWaveletTransform _singletonInstace;

        public static IWaveletTransform Implementation {
            get {
                return _singletonInstace;
            }
        }

        static DB4WaveletTransform()
        {
            _singletonInstace = new DB4WaveletTransform();
        }

        // taken from http://bearcave.com/misl/misl_tech/wavelets/daubechies/index.html
        // I have no idea if this even works 
        void db4FWTSingle(Slice<float> a, Slice<float> dst)
        {
            int n = a.Length;

            if (n < 4)
                return;

            float fourRoot2 = (float)(4 * Math.Sqrt(2));
            float root3 = (float)Math.Sqrt(3);

            float h0 = (1 + root3) / fourRoot2;
            float h1 = (3 + root3) / fourRoot2;
            float h2 = (3 - root3) / fourRoot2;
            float h3 = (1 - root3) / fourRoot2;

            float g0 = h3;
            float g1 = -h2;
            float g2 = h1;
            float g3 = -h0;


            int i, j;
            int half = n >> 1;

            i = 0;
            for (j = 0; j < n - 3; j = j + 2)
            {
                dst[i] = a[j] * h0 + a[j + 1] * h1 + a[j + 2] * h2 + a[j + 3] * h3;
                dst[i + half] = a[j] * g0 + a[j + 1] * g1 + a[j + 2] * g2 + a[j + 3] * g3;
                i++;
            }

            dst[i] = a[n - 2] * h0 + a[n - 1] * h1 + a[0] * h2 + a[1] * h3;
            dst[i + half] = a[n - 2] * g0 + a[n - 1] * g1 + a[0] * g2 + a[1] * g3;
        }

        public void Transform(Slice<float> src, Slice<float> tempBuffer)
        {
            db4FWTSingle(src, tempBuffer);
        }
    }
}
