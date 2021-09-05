using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SongBPMFinder
{
    public class SumSquareDifferencesFTDeltaCalculator : AbstractFTDeltaCalculator
    {
        public SumSquareDifferencesFTDeltaCalculator(int minimumFrequency, int maximumFrequency, bool correctFrequencies)
            : base (minimumFrequency, maximumFrequency, correctFrequencies)
        {
        }

        protected override float deltaInternal(float a, float b, float accumulator, int frequency)
        {
            return accumulator + (a-b)*(a-b);
        }
    }
}
