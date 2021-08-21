using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SongBPMFinder
{
    public static class Asserting
    {
        public static void Assert(bool value)
        {
            if (!value)
            {
                throw new Exception("Something went wrong.");
            }
        }

        public static void Assert(bool value, string message)
        {
            if (!value)
            {
                throw new Exception(message);
            }
        }

        public static void Assert(bool value, Exception ex)
        {
            if (!value)
            {
                throw ex;
            }
        }
    }
}
