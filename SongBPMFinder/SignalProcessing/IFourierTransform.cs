using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SongBPMFinder.Slices;

namespace SongBPMFinder.SignalProcessing
{
    interface IFourierTransform
    {
        void Forward(Slice<float> real, Slice<float> imag);
        void Backward(Slice<float> real, Slice<float> imag);
    }
}
