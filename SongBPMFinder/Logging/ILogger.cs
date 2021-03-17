using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SongBPMFinder.Logging
{
    public interface ILogger
    {
        void Log(string msg);
        void Clear();
    }
}
