using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using SongBPMFinder.Util;

namespace SongBPMFinder.Audio.Timing
{
    public class Timing 
    {

        /// <summary>
        /// Finds a beat within the given window.
        /// If there are multiple, it returns the 'most powerful'
        /// TBH this is very rng.
        /// I would reccomend doing a deep copy of the slice before passing it in,
        /// as all operations done are destructive
        /// which is bad from a programming and software maintenance standpoint, but good from a space/performance one
        /// </summary>
        /// <param name="data">input array</param>
        /// <param name="len">length of the array to consider</param>
        /// <param name="windowStartPos">start of the window</param>
        /// <param name="windowLength">size of the window in samples aka array indices</param>
        /// <returns>Where we found a beat, or -1 if we didnt find anything</returns>
        public static long FindBeat(AudioData audioData, Slice<float> slice, int numLevels = 4)
        {
            Slice<float>[] dwtSlices = new Slice<float>[numLevels];

            for (int i = 0, h = slice.Length; i < numLevels; i++, h /= 2)
            {
                dwtSlices[i] = slice.GetSlice(0, h);
                FloatArrays.HaarFWT(dwtSlices[i]);
            }

            FloatArrays.Abs(slice);

            //Resize DWT slices so that they only keep their detail coefficients
            for (int i = 0; i < numLevels; i++)
            {
                dwtSlices[i] = dwtSlices[i].GetSlice(dwtSlices[i].Length / 2, dwtSlices[i].Length);
            }


            Slice<float>[] downsampleSlices = new Slice<float>[numLevels];

            //each index a the downsampled array will be equal to this amount of time in seconds
            float instant = 0.001f;
            //convert to samples
            int instantSize = audioData.ToSamples(instant);

            int sliceLen = (slice.Length / instantSize);

            for (int i = 0; i < numLevels; i++)
            {
                downsampleSlices[i] = slice.GetSlice(i * sliceLen, (i + 1) * sliceLen);

                int downsampleFactor = instantSize / (QuickMafs.Pow(2, numLevels - i));
                //debug breakpoint
                //int predictedSize = dwtSlices[numLevels - i - 1].Length / downsampleFactor ;

                FloatArrays.DownsampleAverage(dwtSlices[numLevels - i - 1], downsampleSlices[i], downsampleFactor);
            }

            //Subtract means
            for (int i = 1; i < numLevels; i++)
            {
                float average = FloatArrays.Average(downsampleSlices[i], true);
                FloatArrays.Sum(downsampleSlices[i], -average);
            }

            //Add all envelopes onto each other
            for (int i = 1; i < numLevels; i++)
            {
                FloatArrays.Sum(downsampleSlices[0], downsampleSlices[i]);
            }

            Slice<float> result = downsampleSlices[0];
            Slice<float> autocorrelPlacement = downsampleSlices[1];
            //timingPoints.Add(new TimingPoint(120, audioData.ToSeconds(autocorrelPlacement.Start)));
            //timingPoints.Add(new TimingPoint(120, audioData.ToSeconds(autocorrelPlacement.Start + autocorrelPlacement.Length)));

            //Autocorrelation
            //This operation is O(N^2), potentially a bottleneck, which explains the windowed approach used by others
            for (int i = 0; i < result.Length; i++)
            {
                float sum = 0;
                float n = result.Length - i;

                for (int j = 0; j < result.Length - i; j++)
                {
                    sum += result[i] * result[i + j];
                }

                autocorrelPlacement[i] = sum / n;
            }

            long max = FloatArrays.ArgMax(autocorrelPlacement, true);
            //convert back to audio
            long maxAudioPos = max * instantSize;
            return maxAudioPos;
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

            long pos = FindBeat(audioData, data.GetSlice(0, audioData.ToSamples(1.0)).DeepCopy());
            timingPoints.Add(new TimingPoint(120, audioData.ToSeconds(pos)));




            

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
