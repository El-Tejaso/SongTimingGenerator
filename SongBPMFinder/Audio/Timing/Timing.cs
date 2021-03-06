using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SongBPMFinder.Util;

namespace SongBPMFinder.Audio.Timing
{
    public class Timing 
    {

        /// <summary>
        /// Finds a beat within the given window
        /// </summary>
        /// <param name="data">input array</param>
        /// <param name="len">length of the array to consider</param>
        /// <param name="windowStartPos">start of the window</param>
        /// <param name="windowLength">size of the window in samples aka array indices</param>
        /// <returns>Where we found a beat, or -1 if we didnt find anything</returns>
        public static int FindBeat(float[] data, int len, int windowStartPos, int windowLength)
        {
            for(int length = windowLength/2; length > 1; length /= 2)
            {

            }
            return -1;
        }

        public static TimingPointList Analyze(AudioData audioData)
        {
            List<TimingPoint> timingPoints = new List<TimingPoint>();

            float[] dataArray;


            //Delete this line in production
            dataArray = audioData.Data;

            //keep this in production
            /*
            dataArray = new float[audioData.Data.Length];
			Array.Copy(audioData.Data, 0, dataArray, 0, dataArray.Length);
			//*/

            //len = DownsampleAverage(data, len, audioData.Channels);

            Slice<float> data = new Slice<float>(dataArray);


            int numLevels = 4;
            Slice<float>[] dwtSlices = new Slice<float>[numLevels];
            
            for(int i = 0, h = data.Length; i < numLevels; i++, h/=2)
            {
                dwtSlices[i] = data.GetSlice(0, h);
                FloatArrays.HaarFWT(dwtSlices[i]);
            }

            FloatArrays.Abs(data);


            //Resize DWT slices so that they only keep their detail coefficients
            for (int i = 0; i < numLevels; i++)
            {
                dwtSlices[i] = dwtSlices[i].GetSlice(dwtSlices[i].Length / 2, dwtSlices[i].Length);
            }
            
            Slice<float>[] downsampleSlices = new Slice<float>[numLevels];
            //Treat 1 millisecond as a downsampling unit
            //Needs to be a power of two probably
            int instantSize = 1024;
            int sliceLen = data.Length / instantSize;

            for (int i = 0; i < 1; i++)
            {
                downsampleSlices[i] = data.GetSlice(i*sliceLen, (i+1) * sliceLen);
                timingPoints.Add(new TimingPoint(120, audioData.ToSeconds((i + 1) * sliceLen)));

                FloatArrays.DownsampleAverage(dwtSlices[numLevels - i - 1], downsampleSlices[i], instantSize / (QuickMafs.Pow(2, numLevels - i)));
                //float average = FloatArrays.Average(downsampleSlices[i]);
                //FloatArrays.Sum(downsampleSlices[i], -average);
            }
            /*
            //Add all envelopes onto each other
            for (int i = 1; i < numLevels; i++)
            {
                FloatArrays.Sum(downsampleSlices[0], downsampleSlices[i]);
            }

            Slice<float> result = downsampleSlices[0];

            for(int i = 0; i < result.Length; i++)
            {

            }
            */

            //len = DownsampleAverage(data, len, instantSize);
            //len = UpsampleLinear(data, len, instantSize);

            /*
            double tol = 0.001;
            timingPoints = TimingPointList.RemoveDoubles(timingPoints, 0.01);
            timingPoints = TimingPointList.CalculateBpms(timingPoints);
            timingPoints = TimingPointList.Simplify(timingPoints, tol);
            */


            return new TimingPointList(timingPoints, false);
        }
    }
}
