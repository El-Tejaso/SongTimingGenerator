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

    }
}
