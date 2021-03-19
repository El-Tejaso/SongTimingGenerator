using SongBPMFinder.Slices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SongBPMFinder.SignalProcessing.Wavelets
{
    public interface IWaveletTransform
    {
        void Transform(Slice<float> src, Slice<float> tempBuffer);
    }
}
