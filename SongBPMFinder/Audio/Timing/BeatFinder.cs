using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using SongBPMFinder.Util;

namespace SongBPMFinder.Audio.Timing
{
    class BeatFinder
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
        /// <param name="slice">a slice of data from the audio file. </param>
        /// <param name="timingPoints">A debug parameter. delete later</param>
        /// <param name="instant">The time in seconsds considered as the smallest unit. osu! uses 0.001 (aka 1 millisecond), your rhythm game may not</param>
        /// <param name="numLevels">Internal variable relating to the wavelet transform. delete later if not needed</param>
        /// <returns>An integer corresponding to the sample in the slice where the beat occured</returns>
        public static int FindBeat(AudioData audioData, Slice<float> slice, Slice<float> tempBuffer, float instant, int numLevels = 4, bool debug = false)
        {
            Slice<float>[] dwtSlices = new Slice<float>[numLevels];

            int origSliceStart = slice.Start;
            //slice = slice.DeepCopy();

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
            int instantSize = audioData.ToSample(instant);
			//Ensure that I can divide this by 2 a certain number of times
			instantSize = QuickMafs.NearestDivisor(instantSize, QuickMafs.Pow(2, numLevels));

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
            Slice<float> result = downsampleSlices[0];

            for (int i = 1; i < numLevels; i++)
            {
                FloatArrays.Sum(result, downsampleSlices[i]);
            }

            //Autocorrelation
            Slice<float> autocorrelPlacement = slice.GetSlice(sliceLen, sliceLen + sliceLen);
            FloatArrays.Autocorrelate(result, autocorrelPlacement);

            //Form1.Instance.Viewer.StartTime = audioData.IndexToSeconds((origSliceStart + sliceLen + sliceLen/4));
            //Form1.Instance.Viewer.WindowLengthSeconds = audioData.IndexToSeconds(sliceLen / 2);

            float mean = FloatArrays.Average(autocorrelPlacement, false);
            FloatArrays.Sum(autocorrelPlacement, -mean);

            //Normalization step here
            FloatArrays.Normalize(autocorrelPlacement);
            mean = FloatArrays.Average(autocorrelPlacement, false);


            int maxIndex = FloatArrays.ArgMax(autocorrelPlacement, false);
            int maxAudioPos = maxIndex * instantSize;

            #region shitCode2

            if (debug)
            {
                //Draw autocorrelated array
                Slice<float> plotArray = autocorrelPlacement.DeepCopy();

                Form1.Instance.Plot(plotArray, 0);

                List<TimingPoint> debugPoints = new List<TimingPoint>();
                double maxT = audioData.IndexToSeconds(maxIndex);
                debugPoints.Add(new TimingPoint(maxT, maxT, Color.Pink));
                Form1.Instance.AddLines(debugPoints, 0);


                //Draw autocorrelated array again, sorted
                Slice<float> plotArraySorted = plotArray.DeepCopy();
                Array.Sort(plotArraySorted.GetArray());

                Form1.Instance.Plot(plotArraySorted, 1);

                List<TimingPoint> debugPoints2 = new List<TimingPoint>();
                double maxT2 = audioData.IndexToSeconds(FloatArrays.ArgMax(plotArraySorted));
                debugPoints2.Add(new TimingPoint(maxT2, maxT2, Color.Pink));
                Form1.Instance.AddLines(debugPoints2, 1);
            }

            #endregion


            float max = autocorrelPlacement[maxIndex];


            float stdev = FloatArrays.StdDev(autocorrelPlacement);
            float ratio = (max-mean) / stdev;

            if (ratio < 4)
            {
                //just because this is the max sample, doesn't necesarily mean that it is siginificant in any way
                return -1;
            }

            //convert back to audio
            
            return maxAudioPos;
        }

		public static Slice<float> PrepareData(AudioData audioData, bool copy = true)
        {
            float[] dataArray;

            //Delete this line in production
            dataArray = audioData.Data;

            Slice<float> dataOrig = new Slice<float>(dataArray);

            Slice<float> data;

            if (copy)
            {
				//A fresh slice
                data = new Slice<float>(new float[dataOrig.Length / audioData.Channels]);
            } 
            else
            {
				//Points to half of the original data array
                data = dataOrig.GetSlice(0, dataOrig.Length / audioData.Channels);
            }

            //extract 1 channel from original data to a buffer half the length
            FloatArrays.ExtractChannel(dataOrig, data, audioData.Channels, 0);

            //FloatArrays.DownsampleMax(dataOrig, data, audioData.Channels);
            //FloatArrays.DownsampleAverage(dataOrig, data, audioData.Channels);
            return data;
        }


        public static List<TimingPoint> FindAllBeats(AudioData audioData, double windowSize, double beatSize, float resolution)
        {
            Slice<float> data = PrepareData(audioData, true);

            List<TimingPoint> timingPoints = new List<TimingPoint>();

            int windowLength = audioData.ToSample(windowSize);
            int beatWindowLength = audioData.ToSample(beatSize);

            float[] windowBuffer = new float[windowLength];
            Slice<float> tempBuffer = new Slice<float>(new float[windowLength]);

            //*
            int pos = 0;
            while (pos < data.Length)
            {
                double currentTime = audioData.SampleToSeconds(pos);
                
                Slice<float> windowSlice = data.GetSlice(pos, Math.Min(pos + windowLength, data.Length)).DeepCopy(windowBuffer);
                int beatPosition = BeatFinder.FindBeat(audioData, windowSlice, tempBuffer, resolution, 4, false);
                double beatTime = audioData.SampleToSeconds(pos + beatPosition);

                if (beatTime > 1.2)
                {
                    //Breakpoint
                }

                if (beatPosition == -1)
                {
                    //There was no 'beat' in this position
                    pos += windowLength / 4;
                    continue;
                }

                timingPoints.Add(new TimingPoint(120, beatTime));
                pos += beatPosition + beatWindowLength;
            }
            //*/

            return timingPoints;
        }

		public static List<TimingPoint> FindAllBeatsCoalescing(AudioData audioData, double windowSize, double beatSize, float resolution, double coalesceWindow)
        {
            Slice<float> data = PrepareData(audioData, true);

            List<TimingPoint> timingPoints = new List<TimingPoint>();

            int windowLength = audioData.ToSample(windowSize);
            int beatWindowLength = audioData.ToSample(beatSize);

            float[] windowBuffer = new float[windowLength];
            Slice<float> tempBuffer = new Slice<float>(new float[windowLength]);

            int pos = 0;
            while (pos < data.Length)
            {
				//debug reasons
                double currentTime = audioData.SampleToSeconds(pos);
                
                Slice<float> windowSlice = data.GetSlice(pos, Math.Min(pos + windowLength, data.Length)).DeepCopy(windowBuffer);
                int beatPosition = BeatFinder.FindBeat(audioData, windowSlice, tempBuffer, resolution, 4, false);
                double beatTime = audioData.SampleToSeconds(pos + beatPosition);

				pos += windowLength / 6;

                if (beatPosition == -1) continue;

				TimingPointList.AddCoalescing(timingPoints, new TimingPoint(120, beatTime), coalesceWindow);
            }

			List<TimingPoint> cleanList = new List<TimingPoint>();
			for(int i = 0; i < timingPoints.Count; i++){
				if(timingPoints[i].Weight > 2.0){
					cleanList.Add(timingPoints[i]);
				}
			}

            return cleanList;
        }

    }
}
