using System.Windows.Forms;

namespace SongBPMFinder
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
