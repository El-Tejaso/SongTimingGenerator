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

            float[] data;


			//Delete this line in production
			data = audioData.Data;

			//keep this in production
			/*
            data = new float[audioData.Data.Length];
			Array.Copy(audioData.Data, 0, data, 0, data.Length);
			//*/

            int len = data.Length;
            //len = DownsampleAverage(data, len, audioData.Channels);


            for(int i = 0, h = len; i < 4; i++, h /= 2)
            {
                HaarFWT(data, 0, h);
            }

            //len = OnePoleLPF(data, len, -0.5f, 0.5f);
            //len = Abs(data, len);



            int instantSize = (int)(0.02f * audioData.SampleRate);
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
