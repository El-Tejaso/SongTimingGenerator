using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace SongBPMFinder.Logging
{
    public static class Logger
    {
        static ILogger output;
        public static void SetOutput(ILogger newOutput)
        {
            output = newOutput;
            Clear();
        }

        public static void Clear()
        {
            output.Clear();
        }

        public static void Log(string msg)
        {
            output.Log(msg);
            
        }
    }

}
