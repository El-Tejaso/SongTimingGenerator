using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SongBPMFinder.Audio.BeatDetection
{
    public struct BeatData
    {
        public int Position;
        public float RelativeStrength;

        public BeatData(int position, float relativeStrength)
        {
            Position = position;
            RelativeStrength = relativeStrength;
        }
    }
}
