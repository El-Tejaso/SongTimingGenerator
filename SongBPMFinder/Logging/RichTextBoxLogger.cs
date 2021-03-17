using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SongBPMFinder.Logging
{
    class RichTextBoxLogger : ILogger
    {
        RichTextBox output;
        public RichTextBoxLogger(RichTextBox textBox)
        {
            output = textBox;
        }
        public void Clear()
        {
            output.Text = "";
        }

        public void Log(string msg)
        {
            output.AppendText(msg + "\n");
            output.ScrollToCaret();
        }
    }
}
