using System;

namespace SongBPMFinder
{
    class MathUtilF
    {
        public static bool IsIntegerMultiple(double a, double b, double tolerance = 0.000001)
        {
            if (a < b)
                return IsIntegerMultiple(b, a);


            return Math.Abs((a / b) % 1.0) < tolerance;
        }

        public static float Lerp(float a, float b, float t)
        {
            return a + (t * (b - a));
        }

        public static float Clamp(float v, float l, float u)
        {
            return Math.Max(l, Math.Min(v, u));
        }

        public static float Clamp01(float v)
        {
            return Clamp(v, 0f, 1f);
        }

        public static int Pow(int x, int p)
        {
            if (p < 0)
                return x;

            int res = 1;

            while (p > 0)
            {
                res *= x;
                p--;
            }

            return res;
        }

        // Can be used to return a number y such that x/base^exponent will be an integer
        // (as well as everything in between)
        public static int NearestDivisor(int x, int value)
        {
            return (int)(Math.Ceiling((float)x / (float)value) * (float)value);
        }

        public static float MultilpyImaginaryR(float aR, float bR, float aI, float bI)
        {
            return aR * bR - aI * bI;
        }

        public static float MultilpyImaginaryI(float aR, float bR, float aI, float bI)
        {
            return aR * bI + aI * bR;
        }

        public static int NearestPower(int x, int val)
        {
            int num = 0;
            while (x > 1)
            {
                x /= val;
                num++;
            }

            return Pow(val, num);
        }
    }
}
