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
        /// <param name="audioData">the audio file where the slice originated. mainly used for conversion factors</param>
        /// <param name="slice">a slice of data from the audio file. </param>
        /// <param name="timingPoints">A debug parameter. delete later</param>
        /// <param name="instant">The time in seconsds considered as the smallest unit. osu! uses 0.001 (aka 1 millisecond), your rhythm game may not</param>
        /// <param name="numLevels">Internal variable relating to the wavelet transform. delete later if not needed</param>
        /// <returns>An integer corresponding to the sample in the slice where the beat occured</returns>
        public static int FindBeat(AudioData audioData, Slice<float> slice, Slice<float> tempBuffer, float instant, int numLevels = 4, bool debug = false)
        {
			//Discrete wavelet transform
			Slice<float>[] dwtSlices = FloatArrays.HaarFWT(slice, tempBuffer, numLevels);

			//Full wave rectification
            FloatArrays.Abs(slice);


			//Downsampling
            //each index a the downsampled array will be equal to this amount of time in seconds
            //convert to samples
            int instantSize = audioData.ToSample(instant);
			//Ensure that I can divide this by 2 a certain number of times
			instantSize = QuickMafs.NearestDivisor(instantSize, QuickMafs.Pow(2, numLevels));

			Slice<float>[] downsampleSlices = FloatArrays.DownsampleCoefficients(dwtSlices, slice, instantSize);

			//Normalization
            for (int i = 0; i < numLevels; i++)
            {
                //We are actually adding the average. It works for some reason
                float meanI = FloatArrays.Average(downsampleSlices[i], false);
                //FloatArrays.Sum(downsampleSlices[i], meanI);

                //FloatArrays.Normalize(downsampleSlices[i]);
            }

            //Add all envelopes onto each other at downsampleSlices[0]
            Slice<float> summedEnvelopes = downsampleSlices[0];

            for (int i = 1; i < numLevels; i++)
            {
                FloatArrays.Sum(summedEnvelopes, downsampleSlices[i]);
            }

            //Autocorrelation
            Slice<float> autocorrelPlacement = slice.GetSlice(summedEnvelopes.Length, 2 * summedEnvelopes.Length);
            FloatArrays.Autocorrelate(summedEnvelopes, autocorrelPlacement);

            //Do another normalization step
            float mean = FloatArrays.Average(autocorrelPlacement, false);
            FloatArrays.Sum(autocorrelPlacement, -mean);
            FloatArrays.Normalize(autocorrelPlacement);


            int maxIndex = FloatArrays.ArgMax(autocorrelPlacement, false);

            #region DebugCode
            if (debug)
            {
                //Draw autocorrelated array
                Slice<float> plotArray = autocorrelPlacement.DeepCopy();

                Form1.Instance.Plot("Autocorrelated", plotArray, 0);
                List<TimingPoint> debugPoints = new List<TimingPoint>();
                double maxT = audioData.SampleToSeconds(maxIndex);
                debugPoints.Add(new TimingPoint(maxT, maxT, Color.Pink));
                Form1.Instance.AddLines(debugPoints, 0);

                //Draw autocorrelated array again, sorted
                Slice<float> plotArraySorted = plotArray.DeepCopy();
                Array.Sort(plotArraySorted.GetInternalArray());

                Form1.Instance.Plot("Acl sorted", plotArraySorted, 1);

                Slice<float> plotArrayAbsSorted = plotArraySorted.DeepCopy();
                FloatArrays.Abs(plotArrayAbsSorted);
                Array.Sort(plotArrayAbsSorted.GetInternalArray());
                Form1.Instance.Plot("Acl ABS sorted", plotArrayAbsSorted, 2);

				Slice<float> plotArrayDx = plotArray.DeepCopy();
				FloatArrays.Differentiate(plotArrayDx, audioData.SampleRate/instantSize);
				//FloatArrays.Normalize(plotArrayDx);
                Form1.Instance.Plot("Acl dx", plotArrayDx, 3);

				Slice<float> plotArrayDDx = plotArrayDx.DeepCopy();
				FloatArrays.Differentiate(plotArrayDDx, audioData.SampleRate/instantSize);
				//FloatArrays.Normalize(plotArrayDDx);
                Form1.Instance.Plot("Acl ddx", plotArrayDDx, 4);
            }

            #endregion

			//Check if this maximum is significant enough to be a beat

            float max = autocorrelPlacement[maxIndex];
            float av = FloatArrays.Average(autocorrelPlacement);
            float stdDev = FloatArrays.StdDev(autocorrelPlacement);
			float meanAbs = FloatArrays.Average(autocorrelPlacement, true);

            float ratio = (max-av) / meanAbs;

            if (ratio < 8)
            {
                //just because this is the max sample, doesn't necesarily mean that it is siginificant in any way
                return -1;
            }

            //convert back to position in audio
            int maxAudioPos = maxIndex * instantSize;
            return maxAudioPos;
        }

		public static Slice<float> PrepareData(AudioData audioData, bool copy = true)
        {
            Slice<float> data = audioData.GetChannel(0);

            if (copy)
            {
                data = data.DeepCopy();
            }

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


                int beatPosition = FindBeat(audioData, windowSlice, tempBuffer, resolution, 4, false);
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
