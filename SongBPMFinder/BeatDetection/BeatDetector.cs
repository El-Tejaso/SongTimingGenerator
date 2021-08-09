using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SongBPMFinder
{
    public class BeatDetector : IBeatDetector
    {
        public SortedList<Beat> GetEveryBeat(AudioSlice audioSlice)
        {
            SortedList<Beat> beats = new SortedList<Beat>();

            //code here


            return beats;
        }
    }
}
