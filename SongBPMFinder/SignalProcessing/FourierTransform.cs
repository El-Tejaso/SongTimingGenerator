using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SongBPMFinder
{

    public static class FourierTransform
    {
        static float[] sinTable;
        static float[] cosTable;
        const int TABLESIZE = 360;


        static FourierTransform()
        {
            sinTable = new float[TABLESIZE];
            cosTable = new float[TABLESIZE];

            for(int i = 0; i < TABLESIZE; i++)
            {
                double angle = 2 * Math.PI * (i / (double)TABLESIZE);
                sinTable[i] = (float)Math.Sin(angle);
                cosTable[i] = (float)Math.Cos(angle);
            }
        }

        private static float Sin(float x)
        {
            int angle = getAngleIndex(x);
            return sinTable[angle];
        }

        private static float Cos(float x)
        {
            int angle = getAngleIndex(x);
            return cosTable[angle];
        }

        private static int getAngleIndex(float x)
        {
            return (int)Math.Floor(x / (2 * (float)Math.PI) * TABLESIZE) % TABLESIZE;
        }

        private static float ArcTangent2(float y, float x)
        {
            return (float)Math.Atan2(y, x);
        }

        public static void FourierTransformMagnitudesAndPhases(Span<float> input, Span<float> magnitudesDest, Span<float> phasesDest)
        {
            fourierTransformInternal(input, magnitudesDest, phasesDest, true, true);
        }

        public static void FourierTransformMagnitudes(Span<float> input, Span<float> magnitudesDest)
        {
            fourierTransformInternal(input, magnitudesDest, magnitudesDest, false, true);
        }

        public static void FourierTransformPhases(Span<float> input, Span<float> phasesDest)
        {
            fourierTransformInternal(input, phasesDest, phasesDest, true, false);
        }


        private static void fourierTransformInternal(Span<float> input, Span<float> magnitudesDest, Span<float> phasesDest, bool calculatePhases, bool calculateMagnitudes)
        {
            SpanFunctional.AssertEqualLength(magnitudesDest, phasesDest);

            naiveFFT(input, magnitudesDest, phasesDest, calculatePhases, calculateMagnitudes);
        }


        private static void fastFFT(Span<float> input, Span<float> magnitudesDest, Span<float> phasesDest, bool calculatePhases, bool calculateMagnitudes)
        {

        }


        /// <summary>
        /// Implemented with Naive intuition from the 3Blue1Brown video.
        /// Awesome video, would have been cool to know that the magnitude of the 'center of mass' vector 
        /// is actually the strength of a frequency, and the angle is the phase.
        ///
        /// Runs in O(input_size^2)
        /// </summary>
        private static void naiveFFT(Span<float> input, Span<float> magnitudesDest, Span<float> phasesDest, bool calculatePhases, bool calculateMagnitudes)
        {
            int n = Math.Min(magnitudesDest.Length, input.Length / 2);

            for (int i = 0; i < n; i++)
            {
                float period = i + 2;

                float x = 0;
                float y = 0;

                for (int pos = 0; pos < n; pos++)
                {
                    float angle = (2f * (float)Math.PI) * (pos / period);
                    float magnitude = input[pos];

                    x += magnitude * Cos(angle);
                    y += magnitude * Sin(angle);
                }

                if (calculateMagnitudes)
                {
                    magnitudesDest[i] = (float)Math.Sqrt(x * x + y * y);
                }

                if (calculatePhases)
                {
                    phasesDest[i] = ArcTangent2(y, x);
                }
            }
        }
    }
}
