using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SongBPMFinder.BeatDetection.Differentiators
{
    public class CoherenceFTDeltaCalculator : AbstractFTDeltaCalculator
    {
        public CoherenceFTDeltaCalculator(int minimumFrequency, int maximumFrequency, bool correctFrequencies)
            : base(minimumFrequency, maximumFrequency, correctFrequencies)
        {
            this.correctFrequencies = correctFrequencies;
        }

        protected override float deltaInternal(float a, float b, float acc, int frequency)
        {
            
            return acc;
        }
    }
}
