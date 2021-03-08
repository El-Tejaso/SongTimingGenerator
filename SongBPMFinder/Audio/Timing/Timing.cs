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

            int origSliceStart = slice.Start;
            slice = slice.DeepCopy();

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

            for (int i = 0; i < numLevels; i++)
            {
                //Subtract means
                float average = FloatArrays.Average(downsampleSlices[i], false);
                FloatArrays.Sum(downsampleSlices[i], average);

                //remove negative values
                //FloatArrays.Max(downsampleSlices[i], 0);
            }
            

            //Add all envelopes onto each other at slices[0]
            for (int i = 1; i < numLevels; i++)
            {
                FloatArrays.Sum(downsampleSlices[0], downsampleSlices[i]);
            }


            //Autocorrelation
            Slice<float> result = downsampleSlices[0];
            Slice<float> autocorrelPlacement = slice.GetSlice(sliceLen, sliceLen + sliceLen / 2);
            FloatArrays.Autocorrelate(result, autocorrelPlacement);


            timingPoints.Add(new TimingPoint(120, audioData.ToSeconds(origSliceStart), Color.Blue));
            timingPoints.Add(new TimingPoint(120, audioData.ToSeconds(origSliceStart + sliceLen), Color.Blue));
            timingPoints.Add(new TimingPoint(120, audioData.ToSeconds(origSliceStart + 2*sliceLen), Color.Blue));


            float mean = FloatArrays.Average(autocorrelPlacement, false);
            FloatArrays.Sum(autocorrelPlacement, -mean);

            int maxIndex = FloatArrays.ArgMax(autocorrelPlacement, false);
            float max = autocorrelPlacement[maxIndex];

            float stdev = FloatArrays.StdDev(autocorrelPlacement, false);
            float ratio = max / stdev;

            if (ratio < 4)
            {   
                //just because this is the max sample, doesn't necesarily mean that it is siginificant in any way
                //return -1;
            }

            //convert back to audio
            int maxAudioPos = maxIndex * instantSize;
            return maxAudioPos;
        }


        public static int FindBeatNoDWT(AudioData audioData, Slice<float> slice, Slice<float> tempBuffer, List<TimingPoint> timingPoints, float instant = 0.001f, int numLevels = 4)
        {
            int origSliceStart = slice.Start;
            //slice = slice.DeepCopy();

            //FWR
            FloatArrays.Abs(slice);


            //DOWNSAMPLE
            int instantSize = audioData.ToSamples(instant);
            int downsampleFactor = instantSize;
            int downsampledSize= slice.Length / downsampleFactor;

            Slice<float> downsampleSlice = slice.GetSlice(0, downsampledSize);
            FloatArrays.DownsampleAverage(slice, downsampleSlice, downsampleFactor);

           
            //NORMALIZE
            float average = FloatArrays.Average(downsampleSlice, false);
            FloatArrays.Sum(downsampleSlice, -average);


            Slice<float> autocorrelPlacement = slice.GetSlice(downsampledSize, 2 * downsampledSize);
            FloatArrays.Autocorrelate(downsampleSlice, autocorrelPlacement);

            autocorrelPlacement = autocorrelPlacement.GetSlice(0, autocorrelPlacement.Length / 2);

            timingPoints.Add(new TimingPoint(120, audioData.ToSeconds(origSliceStart), Color.Blue));

            timingPoints.Add(new TimingPoint(120, audioData.ToSeconds(origSliceStart + autocorrelPlacement.Start), Color.Blue));
            timingPoints.Add(new TimingPoint(120, audioData.ToSeconds(origSliceStart + autocorrelPlacement.Start + autocorrelPlacement.Length), Color.Blue));


            float mean = FloatArrays.Average(autocorrelPlacement, false);
            FloatArrays.Sum(autocorrelPlacement, -mean);

            int maxIndex = FloatArrays.ArgMax(autocorrelPlacement, false);
            float max = autocorrelPlacement[maxIndex];

            float stdev = FloatArrays.StdDev(autocorrelPlacement, false);
            float ratio = max / stdev;

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


            int doubleWindowLength = audioData.ToSamples(0.2) * 2;
            int beatWindow = audioData.ToSamples(0.01);

            float[] windowBuffer = new float[doubleWindowLength];
            Slice<float> tempBuffer = new Slice<float>(new float[doubleWindowLength]);

            float resolution = 0.001f;

            /*
            int pos = 0;
            while(pos < data.Length)
            {
                if(audioData.ToSeconds(pos) > 2.4)
                {

                }

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

            

            /*
            //double t = 0.52;
            //double t = 0.3;
            //double t = 22.85;
            int pos = audioData.ToSamples(0);
            //Slice<float> s = data.GetSlice(pos, pos + doubleWindowLength);
            Slice<float> s = data;

            //timingPoints.Add(new TimingPoint(120, audioData.ToSeconds(s.Start), Color.Blue));
            //timingPoints.Add(new TimingPoint(120, audioData.ToSeconds(s.Start + s.Length), Color.Blue));

            int beatPosition = FindBeat(audioData, s, s.DeepCopy(), timingPoints, resolution, 4);
            timingPoints.Add(new TimingPoint(120, audioData.ToSeconds(pos + beatPosition), Color.Green));

            timingPoints.Add(new TimingPoint(120, audioData.ToSeconds(pos + 0)));
            timingPoints.Add(new TimingPoint(120, audioData.ToSeconds(pos + doubleWindowLength/2)));
            timingPoints.Add(new TimingPoint(120, audioData.ToSeconds(pos + doubleWindowLength)));

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
