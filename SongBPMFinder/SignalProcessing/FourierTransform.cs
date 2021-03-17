using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using SongBPMFinder.Util;

namespace SongBPMFinder.SignalProcessing
{
    //This class is currently not working. Please do not use any of the methods contained in this class and instead use the AccordFourierTransform class
    class FourierTransform
    {
		//Performs the fourier transform.
		//src and dst may not overlap unless they overlap perfectly (i.e in place transform)
        public static void FFTForward(Slice<float> srcR, Slice<float> srcI, Slice<float> dstR, Slice<float> dstI)
        {
            fastFFT(srcR, srcI, dstR, dstI);
        }

		//Performs the fourier transform backwards and applies inverse scaling
		//src and dst may not overlap unless they overlap perfectly (i.e in place transform)
        public static void FFTBackward(Slice<float> srcR, Slice<float> srcI, Slice<float> dstR, Slice<float> dstI)
        {
            fastFFT(srcI, srcR, dstR, dstI);

            SliceMathf.Divide(dstR, dstR.Length);
            SliceMathf.Divide(dstI, dstI.Length);
        }

        //This implementation is based on this article: https://jakevdp.github.io/blog/2013/08/28/understanding-the-fft/
		//and as such, may not be as efficient as possible
        private static void fastFFT(Slice<float> srcR, Slice<float> srcI, Slice<float> dstR, Slice<float> dstI)
        {
            if(srcR.Length < 16)
            {
                slowFFT(srcR, srcI, dstR, dstI);
                return;
            }

			//split the source and destination arrays into 2 based on even indices
            Slice<float> evenRSrc = srcR.GetSlice(0, srcR.Length - 1, 2);
            Slice<float> evenRDst = dstR.GetSlice(0, srcR.Length - 1, 2);
            Slice<float> evenISrc = srcI.GetSlice(0, srcI.Length - 1, 2);
            Slice<float> evenIDst = dstI.GetSlice(0, srcI.Length - 1, 2);

            fastFFT(evenRSrc, evenISrc, evenRDst, evenIDst);

			//split the source and destination arrays into 2 again but based on odd indices this time
			Slice<float> oddRSrc = srcR.GetSlice(1, srcR.Length, 2);
            Slice<float> oddRDst = dstR.GetSlice(1, srcR.Length, 2);
            Slice<float> oddISrc = srcI.GetSlice(1, srcI.Length, 2);
            Slice<float> oddIDst = dstI.GetSlice(1, srcI.Length, 2);

            fastFFT(oddRSrc, oddISrc, oddRDst, oddIDst);

			//Calculate a factor
            int n = srcR.Length;
			int halfN = n/2;
            for (int k = 0; k < halfN; k++)
            {
                double factorAngle = -2 * Math.PI * k / (double)n;
                float factorR = (float)Math.Cos(factorAngle);
                float factorI = (float)Math.Sin(factorAngle);

				//Store these values here before they get overwritten
                float evenRDstK = evenRDst[k];
                float evenIDstK = evenIDst[k];
                float oddRDstK = oddRDst[k];
                float oddIDstK = oddIDst[k];
			
                dstR[k] = evenRDstK + factorR * oddRDstK;
                dstI[k] = evenIDstK + factorI * oddIDstK;

				//Reuse the values in the second half of the array in this for-loop itself
				factorAngle = -2 * Math.PI * (k + halfN) / (double)n;
                factorR = (float)Math.Cos(factorAngle);
                factorI = (float)Math.Sin(factorAngle);

                dstR[k + halfN] = evenRDstK + factorR * oddRDstK;
                dstI[k + halfN] = evenIDstK + factorI * oddIDstK;
            }
        }

        //The source arrays and the destination arrays may not be the same
        private static void slowFFT(Slice<float> srcR, Slice<float> srcI, Slice<float> dstR, Slice<float> dstI)
        {
            int n = srcR.Length;

            float[] resR = new float[dstR.Length];
            float[] resI = new float[dstI.Length];

            for (int k = 0; k < n; k++)
            {
                float R = 0;
                float im = 0;

                for(int t = 0; t < n; t++)
                {
                    double angle = 2 * Math.PI * t * k / (double)n;
                    R += (float)(+srcR[t] * Math.Cos(angle) + srcI[t] * Math.Sin(angle));
                    im += (float)(-srcR[t] * Math.Sin(angle) + srcI[t] * Math.Cos(angle));
                }

                resR[k] = R;
                resI[k] = im;
            }

            for (int k = 0; k < n; k++)
            {
                dstR[k] = resR[k];
                dstI[k] = resI[k];
            }
        }
    }
}
