using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SongBPMFinder
{
    //TODO: delete this later
    public class TestTimingGenerator : ITimingGenerator
    {
        //Just spit out whereever the beats are, generate no real timing
        public TimingPointList GenerateTiming(SortedList<Beat> beats)
        {
            SortedList<TimingPoint> timingPointList = new SortedList<TimingPoint>();

            //code here
            for(int i = 0; i < beats.Count; i++)
            {
                timingPointList.Add(new TimingPoint(120, beats[i].Time));
            }

            return new TimingPointList(timingPointList);
        }
    }
}
