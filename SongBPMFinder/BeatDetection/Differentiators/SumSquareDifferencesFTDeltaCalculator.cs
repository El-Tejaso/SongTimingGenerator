using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SongBPMFinder
{
    public class SumSquareDifferencesFTDeltaCalculator : AbstractFTDeltaCalculator
    {
        public SumSquareDifferencesFTDeltaCalculator(int minimumFrequency, int maximumFrequency)
            : base (minimumFrequency, maximumFrequency)
        {
        }

        protected override float deltaInternal(float a, float b, float accumulator)
        {
            return accumulator + (a-b)*(a-b);
        }
    }
}
