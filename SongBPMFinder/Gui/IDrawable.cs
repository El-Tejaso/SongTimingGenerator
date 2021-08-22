using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SongBPMFinder
{
    public interface IDrawable
    {
        void Draw(Control control, WaveformCoordinates coordinates, Graphics g);
    }
}
