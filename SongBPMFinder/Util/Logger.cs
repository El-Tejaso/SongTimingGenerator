using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace SongBPMFinder.Util
{
    public class Logger
    {
        static RichTextBox output;
        public static void SetOutput(RichTextBox newOutput)
        {
            output = newOutput;
            ClearOutput();
        }

        public static void ClearOutput()
        {
            output.Text = "";
        }

        public static void Log(string msg)
        {
            output.AppendText(msg + "\n");
            output.ScrollToCaret();
        }
    }

}
