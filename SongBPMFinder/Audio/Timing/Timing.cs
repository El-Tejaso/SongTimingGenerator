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
        // This function assumes that input.length=2^n, n>1
        static float[] dwt(float[] input)
        {
            float[] output = new float[input.Length];

            for (int length = input.Length / 2; ; length = length / 2)
            {
                // length is the current length of the working area of the output array.
                // length starts at half of the array size and every iteration is halved until it is 1.
                for (int i = 0; i < length; ++i)
                {
                    float sum = input[i * 2] + input[i * 2 + 1];
                    float difference = input[i * 2] - input[i * 2 + 1];
                    output[i] = sum;
                    output[length + i] = difference;
                }
                if (length <= 1)
                {
                    if (length == 0)
                    {
                        Logger.Log("tjats weierd. Length == 0");
                    }
                    return output;
                }

                //Swap arrays to do next iteration
                Array.Copy(output, 0, input, 0, length);
            }
        }

        static int DownsampleAverage(float[] x, int len, int channels)
        {
            for (int i = 0; i < len / channels; i++)
            {
                float sum = 0;
                for(int j = 0; j < channels; j++)
                {
                    sum += x[i*channels + j];
                }

                x[i] = sum / (float)channels;
            }
            
            return len/channels;
        }

        static int Abs(float[] x, int len){
			for(int i = 0; i < len; i++){
				x [i] = Math.Abs(x[i]);
			}
			return len;
		}

        static float Max(float[] x, int len)
        {
            float max = x[0];
            for (int i = 1; i < len; i++)
            {
                if (x[i] > max) max = x[i];
            }
            return max;
        }

        static float Min(float[] x, int len)
        {
            float min = x[0];
            for (int i = 1; i < len; i++)
            {
                if (x[i] < min) min = x[i];
            }
            return min;
        }

        /// <summary>
        /// Generates a Sliding window average in place.
        /// Returns the new length of the array
        /// </summary>
        /// <param name="x">The array to operate on</param>
        /// <param name="len">the length of the array</param>
        /// <param name="size">The size of the window</param>
        /// <returns></returns>
        static int SlidingWindowAverage(float[] x, int len, int size, float exponent)
        {
            float sum = 0;
            for(int i = 0; i < size; i++)
            {
                sum += x[i];
            }

            for (int i = size; i < len - size; i++)
            {
                float temp = x[i-size];
                x[i-size] = (float)Math.Pow(sum,exponent)/(float)size;
                sum -= temp;
                sum += x[i];
            }

            return len - size + 1;
        }

        public static int Differentiate(float[] x, int len, float XUnits)
        {
            for(int i = 0; i < len-1; i++)
            {
                float x2 = x[i + 1];
                float x1 = x[i];
                float dX = x2 - x1;
                x[i] = dX/ XUnits;
            }
            return len-1;
        }

        public static TimingPointList Analyze(AudioData audioData)
        {
            List<TimingPoint> timingPoints = new List<TimingPoint>();

            float[] data;

			//Delete this line in production
			data = audioData.Data;

			//keep this in production
			//*
            data = new float[audioData.Data.Length];
			Array.Copy(audioData.Data, 0, data, 0, data.Length);
			//*/


            int len = data.Length;
            len = DownsampleAverage(data, len, audioData.Channels);


            //int instantSize = (int)(0.005f * audioData.SampleRate);
            int instantSize = 1024;
            int averageSize = (int)(1f * audioData.SampleRate);


			float[] averageEnergy = new float[len];
			Array.Copy(data, 0, averageEnergy, 0, len);
            float sum = 0;
            for(int i = 0;i < averageEnergy.Length; i++)
            {
                sum += Math.Abs(averageEnergy[i]);
            }


			float[] instantEnergy = new float[len];
            Array.Copy(data, 0, instantEnergy, 0, len);

            int lenAverage = SlidingWindowAverage(averageEnergy, len, averageSize, 3);
			int lenInstant = SlidingWindowAverage(instantEnergy, len, instantSize, 3);

			int alignmentOffset = averageSize/2 - instantSize/2;
			
			for(int i = 0; i < lenAverage; i++){
                if (i + alignmentOffset >= instantEnergy.Length) break;

                float ae = averageEnergy[i];
                float ie = instantEnergy[i + alignmentOffset];
                float res = Math.Abs(ie/(Math.Abs(ae)+1f));
                data[i] = res;
			}

            float max = Max(data,len);

            len = lenAverage;

            
            for(int i = 0; i < len; i++)
            {
                if (data[i] > 0.5 * max)
                {
                    timingPoints.Add(new TimingPoint(120, audioData.ToSeconds(i+instantSize/2) * audioData.Channels));
                    i += instantSize;
                }
            }


            double tol = 0.001;
            timingPoints = TimingPointList.RemoveDoubles(timingPoints, tol);
            timingPoints = TimingPointList.CalculateBpms(timingPoints);
            timingPoints = TimingPointList.Simplify(timingPoints, tol);

            return new TimingPointList(timingPoints);
        }
    }
}
