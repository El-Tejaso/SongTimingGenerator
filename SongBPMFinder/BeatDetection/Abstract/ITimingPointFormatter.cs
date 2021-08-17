using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SongBPMFinder
{
    public interface ITimingPointFormatter
    {
        string FormatTiming(TimingPointList timingPoints);
    }
}
