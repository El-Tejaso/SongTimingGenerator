using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SongBPMFinder.Util
{
    class QuickMafs
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
            if (p < 0) return x;

            int res = 1;

            while (p > 0)
            {
                res *= x;
                p--;
            }

            return res;
        }
    }
}
