using SongBPMFinder.Slices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SongBPMFinder.SignalProcessing
{
    //Note: Open to modification
    public static class FourierTransform
    {
        static IFourierTransform _fourierTransformObject;

        static FourierTransform()
        {
            //TODO: Open Close principle
            _fourierTransformObject = new AccordFourierTransform();
            //_fourierTransformObject = new HomemadeFourierTransform();
        }

        public static void Forward(Slice<float> real, Slice<float> imag)
        {
            _fourierTransformObject.Forward(real, imag);
        }

        public static void Backward(Slice<float> real, Slice<float> imag)
        {
            _fourierTransformObject.Backward(real, imag);
        }
    }
}
