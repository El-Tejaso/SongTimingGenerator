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

        static int DownsampleAverage(float[] x, int len, int samples)
        {
            for (int i = 0; i < len / samples; i++)
            {
                float sum = 0;
                for(int j = 0; j < samples; j++)
                {
                    if ((i * samples + j) > len)
                        break;
                    sum += x[i* samples + j];
                }

                x[i] = sum / (float)samples;
            }
            
            return len/ samples;
        }


        static int DownsampleMax(float[] x, int len, int samples)
        {
            for (int i = 0; i < len / samples; i++)
            {
                float max = 0;
                for (int j = 0; j < samples; j++)
                {
                    if ((i * samples + j) > len)
                        break;
                    max = Math.Max(max, x[i * samples + j]);
                }

                x[i] = max;
            }

            return len / samples;
        }

        static int UpsampleLinear(float[] x, int len, int multiple)
        {
            for (int i = len; i > 0; i--)
            {
                int a = ((i - 1) * multiple);
                int b = i * multiple;
                for (int j = a; j < b; j++)
                {
                    if (j > x.Length) break;

                    float t = (j-a) / (float)(b - a);
                    float val = QuickMafs.Lerp(x[i - 1], x[i], t);
                    x[j] = val;
                }
            }

            return Math.Min(len * multiple, x.Length);
        }

        static int Abs(float[] x, int len){
			for(int i = 0; i < len; i++){
				x [i] = Math.Abs(x[i]);
			}
			return len;
		}

        static float Max(float[] x, int len, bool abs = false)
        {
            float max = x[0];
            for (int i = 1; i < len; i++)
            {
                float xi = x[i];
                if (abs) xi = Math.Abs(xi);
                if (xi > max) max = x[i];
            }
            return max;
        }

        static float Min(float[] x, int len, bool abs = false)
        {
            float min = x[0];
            for (int i = 1; i < len; i++)
            {
                float xi = x[i];
                if (abs) xi = Math.Abs(xi);
                if (xi < min) min = xi;
            }
            return min;
        }


        static float Average(float[] x, int len, bool abs = false)
        {
            float average = 0;
            for (int i = 1; i < len; i++)
            {
                float xi = x[i];
                if (abs) xi = Math.Abs(xi);
                average += x[i] / (float)len;
            }
            return average;
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

        public static int Differentiate(float[] x, int len, float notchesPerUnit)
        {
            for(int i = 0; i < len-1; i++)
            {
                float x2 = x[i + 1];
                float x1 = x[i];
                float dX = x2 - x1;
                x[i] = notchesPerUnit * dX;
            }
            return len-1;
        }

        //Doesn't work for inflection points
        public static int FindZeroes(float[] x, int len, bool upwards, bool downwards)
        {
            bool above = x[0] > 0;
            int lastIndex = 0;
            for (int i = 1; i < len; i++)
            {
                float x1 = x[i];
                if ((above) && (x1 < 0))
                {
                    if (downwards)
                    {
                        x[lastIndex] = i; 
                        lastIndex++;
                    }
                    above = false;
                }
                else if ((!above) && (x1 > 0))
                {
                    if (upwards)
                    {
                        x[lastIndex] = i;
                        lastIndex++;
                    }
                    above = true;
                }
            }
            return lastIndex;
        }

        public static int Divide(float[] x, int len, float value)
        {
            for (int i = 1; i < len - 1; i++)
            {
                x[i] = x[i] / value;
            }

            return len;
        }

        public static int Normalize(float[] x, int len)
        {
            float max = Max(x, len, true);
            return Divide(x, len, max);
        }

        public static int CalculateRatio(float[] output, float[] bufferA, float[] bufferB, int offset, int len)
        {
            for(int i = 0; i < len; i++)
            {
                output[i] = bufferA[i+offset] / bufferB[i];
            }
            return len;
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


            int instantSize = (int)(0.02f * audioData.SampleRate);
            //int instantSize = 1024;
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

            //*
            for (int i = 0; i < lenAverage; i++){
                if (i + alignmentOffset >= instantEnergy.Length) break;

                float ae = averageEnergy[i];
                float ie = instantEnergy[i + alignmentOffset];
                float res = Math.Abs(ie/(Math.Abs(ae)+1f));
                data[i] = res;
			}
            //*/

            len = lenAverage;

            Normalize(data, len);

            //Get envelope
            //len = DownsampleMax(data, len, instantSize);

            //Find peaks

            //len = Differentiate(data, len, 0.001f);



            len = DownsampleMax(data, len, 2 * instantSize);
            len = UpsampleLinear(data, len, 2 * instantSize);


            ///*
            //Derivative loop

            len = Differentiate(data, len, audioData.SampleRate);
            bool above = data[0] > 0;
            float derivativeTolerance = 0.2f;

            for (int i = 1; i < len; i++)
            {
                float x1 = data[i];

                if (above)
                {
                    if(x1 < -derivativeTolerance)
                    {
                        //- -> +
                        above = false;
                        timingPoints.Add(new TimingPoint(120, audioData.Channels * audioData.ToSeconds(((i + alignmentOffset)))));
                    } 
                }
                else
                {
                    if(x1 > derivativeTolerance)
                    {
                        above = true;
                    } 
                }
            }
            //*/

            /*
            //Threshold loop
            for (int i = 0; i < len; i++)
            {
                if (data[i] > 0.3f)
                {
                    timingPoints.Add(new TimingPoint(120, audioData.ToSeconds((int)(i*instantSize)+instantSize/2) * audioData.Channels));
                    i += instantSize;
                }
            }
            //*/


            /*
            for (int i = 0; i < len; i++)
            {
                timingPoints.Add(new TimingPoint(120, audioData.ToSeconds(i + instantSize / 2) * audioData.Channels));
                i += instantSize;
            }
			//*/


            double tol = 0.001;
            timingPoints = TimingPointList.RemoveDoubles(timingPoints, 0.01);
            timingPoints = TimingPointList.CalculateBpms(timingPoints);
            timingPoints = TimingPointList.Simplify(timingPoints, tol);

            return new TimingPointList(timingPoints);
        }
    }
}
