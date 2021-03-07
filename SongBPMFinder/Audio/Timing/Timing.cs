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
        /// Finds a beat within the given sample window.
        /// If there are multiple, it returns the 'most powerful'
        /// TBH this is very rng.
        /// I would reccomend doing a deep copy of the slice before passing it in,
        /// as all operations done are destructive
        /// and you may want to preserve other things in the original array (required if using this function multiple times)
        /// </summary>
        /// <param name="audioData">the audio file where the slice originated</param>
        /// <param name="slice">a slice of data from the audio file. 
        /// **IMPORTANT**: the beat will only be searched for in the first half, as the second half will be used
        /// for autocorrelation</param>
        /// <param name="timingPoints">A debug parameter. delete later</param>
        /// <param name="instant">The time in seconsds considered as the smallest unit. osu! uses 0.001 (aka 1 millisecond), your rhythm game may not</param>
        /// <param name="numLevels">Internal variable relating to the wavelet transform. delete later if not needed</param>
        /// <returns>An integer corresponding to the sample in the slice where the beat occured</returns>
        public static int FindBeat(AudioData audioData, Slice<float> slice, Slice<float> tempBuffer, List<TimingPoint> timingPoints, float instant = 0.001f, int numLevels = 4)
        {
            Slice<float>[] dwtSlices = new Slice<float>[numLevels];

            for (int i = 0, h = slice.Length; i < numLevels; i++, h /= 2)
            {
                dwtSlices[i] = slice.GetSlice(0, h);
                FloatArrays.HaarFWT(dwtSlices[i], tempBuffer);
            }

            FloatArrays.Abs(slice);

            //Resize DWT slices so that they only keep their detail coefficients
            for (int i = 0; i < numLevels; i++)
            {
                dwtSlices[i] = dwtSlices[i].GetSlice(dwtSlices[i].Length / 2, dwtSlices[i].Length);
            }


            Slice<float>[] downsampleSlices = new Slice<float>[numLevels];

            //each index a the downsampled array will be equal to this amount of time in seconds
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

            //Can't do downsampleSlices[1] if numLevels == 1
            Slice<float> autocorrelPlacement = slice.GetSlice(sliceLen, 2 * sliceLen);

            timingPoints.Add(new TimingPoint(120, audioData.ToSeconds(autocorrelPlacement.Start), Color.Blue));
            timingPoints.Add(new TimingPoint(120, audioData.ToSeconds(autocorrelPlacement.Start + autocorrelPlacement.Length), Color.Blue));

            //Autocorrelation
            //This operation is O(N^2), potentially a bottleneck, which explains the windowed approach used by others
            for (int i = 0; i < result.Length/2; i++)
            {
                float sum = 0;
                float n = result.Length - i;

                for (int j = 0; j < result.Length/2; j++)
                {
                    sum += result[i] * result[i + j];
                }

                autocorrelPlacement[i] = sum / n;
            }

            autocorrelPlacement = autocorrelPlacement.GetSlice(0, autocorrelPlacement.Length / 2);

            int maxIndex = FloatArrays.ArgMax(autocorrelPlacement, true);
            float stdev = FloatArrays.StdDev(autocorrelPlacement, true);
            float mean = FloatArrays.Average(autocorrelPlacement, true);
            float max = autocorrelPlacement[maxIndex];
            float ratio = (max - mean) / stdev;

            if (ratio < 4)
            {   
                //just because this is the max sample, doesn't necesarily mean that it is siginificant in any way
                //return -1;
            }

            //convert back to audio
            int maxAudioPos = maxIndex * instantSize;
            return maxAudioPos;
        }

        /*
        public static int FindBeat2(AudioData audioData, Slice<float> slice, Slice<float> tempBuffer, List<TimingPoint> timingPoints, float instant = 0.001f, int numLevels = 4)
        {
            FloatArrays.DownsampleAverage(slice, tempBuffer, audioData.SampleRate);

            return -1;
        }
        */

        public static TimingPointList GenerateMultiBPMTiming(AudioData audioData)
        {
            List<TimingPoint> timingPoints = new List<TimingPoint>();

            float[] dataArray;


            //Delete this line in production
            dataArray = audioData.Data;

            Slice<float> data = new Slice<float>(dataArray);


            int doubleWindowLength = audioData.ToSamples(2) * 2;
            int beatWindow = audioData.ToSamples(0.01);

            float[] windowBuffer = new float[doubleWindowLength];
            Slice<float> tempBuffer = new Slice<float>(new float[doubleWindowLength]);

            float resolution = 0.001f;

            /*
            int pos = 0;
            while(pos < data.Length)
            {
                Slice<float> windowSlice = data.GetSlice(pos, Math.Min(pos + doubleWindowLength, data.Length)).DeepCopy(windowBuffer);
                int beatPosition = FindBeat(audioData, windowSlice, tempBuffer, timingPoints, resolution, 4);

                if(beatPosition == -1)
                {
                    //There was no 'beat' in this position
                    pos += doubleWindowLength / 4;
                    continue;
                }

                timingPoints.Add(new TimingPoint(120, audioData.ToSeconds(pos + beatPosition)));
                pos += beatPosition + beatWindow;
            }
            //*/

            //*
            double t = 0.0;
            //Slice<float> s = data.GetSlice(audioData.ToSamples(t), audioData.ToSamples(t) + doubleWindowLength);
            int beatPosition = FindBeat(audioData, data, data.DeepCopy(), timingPoints, resolution, 4);
            //*/



            /*
            double tol = 0.001;
            timingPoints = TimingPointList.RemoveDoubles(timingPoints, 0.01);
            timingPoints = TimingPointList.CalculateBpms(timingPoints);
            timingPoints = TimingPointList.Simplify(timingPoints, tol);
            //*/

            return new TimingPointList(timingPoints, false);
        }
    }
}
