using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SongBPMFinder.Util;

namespace SongBPMFinder.Audio.Timing
{
    //convenience class for working with arrays in place
    public struct Slice<T>
    {
        int start;
        int len;
        T[] array;

        public int Length => len;

        //Mainly for debugging purposes
        public int Start => start; 

        public T this[int index] {
            get { 
                if (start + index >= len)
                {
                    //breakpoint
                }

                if (start + index >= array.Length)
                {
                    //breakpoint
                }

                return array[start + index]; 
            }
            set { array[start + index] = value; }
        }

        public Slice(T[] arr)
        {
            array = arr;
            this.start = 0;
            this.len = arr.Length;
        }

        public Slice<T> GetSlice(int start, int end)
        {
            if(end > array.Length)
            {
                //Breakpoint
            }
            return new Slice<T>(array, this.start + start, end);
        }

        public Slice(T[] arr, int start, int end)
        {
            array = arr;
            this.start = start;
            this.len = end - start;
        }

        public Slice<T> DeepCopy()
        {
            T[] arr = new T[len];
            for(int i = 0; i < arr.Length; i++)
            {
                arr[i] = array[start + i];
            }

            return new Slice<T>(arr);
        }
    }

    /// <summary>
    /// A static class for performing operations on large arrays in-place.
    /// As we can be working with GB-size arrays, we should perform as few copys as possible
    /// </summary>
    class FloatArrays
    {
        //Taken from https://github.com/accord-net/framework/blob/development/Sources/Accord.Math/Wavelets/Haar.cs
        public static void HaarFWT(Slice<float> data)
        {
            Slice<float> temp = data.DeepCopy();

            float w0 = 0.5f;
            float w1 = -0.5f;
            float s0 = 0.5f;
            float s1 = 0.5f;

            int h = data.Length / 2;

            for (int i = 0; i < h; i++)
            {
                int k = (i * 2);
                temp[i] = data[k] * s0 + data[k + 1] * s1;
                temp[i + h] = data[k] * w0 + data[k + 1] * w1;
            }

            for (int i = 0; i < data.Length; i++)
                data[i] = temp[i];
        }

        //Downsamples by an exact number of samples. (the last sample may not be exact)
        //Src and dst may also overlap, as long as dst starts at or before where x starts
        public static void DownsampleAverage(Slice<float> x, Slice<float> dst, int samples)
        {
            if(x.Length < dst.Length)
            {
                Logger.Log("This isnt downsampling");
                return;
            }

            int n = x.Length / samples;
            for (int i = 0; i < n; i++)
            {
                float sum = 0;
                bool shouldEnd = false;

                for (int j = 0; j < samples; j++)
                {
                    if ((i * samples + j) >= x.Length)
                    {
                        shouldEnd = true;
                        break;
                    }
                    
                    sum += x[i * samples + j];
                }

                if (shouldEnd)
                    break;

                dst[i] = sum / (float)samples;
            }
        }


        public static Slice<float> DownsampleMax(Slice<float> x, int samples)
        {
            for (int i = 0; i < x.Length / samples; i++)
            {
                float max = 0;
                for (int j = 0; j < samples; j++)
                {
                    if ((i * samples + j) > x.Length)
                        break;
                    max = Math.Max(max, x[i * samples + j]);
                }

                x[i] = max;
            }

            return x.GetSlice(0, x.Length / samples);
        }

        public static void Sum(Slice<float> src, Slice<float> dst)
        {
            for(int i = 0; i < src.Length; i++)
            {
                if (i > dst.Length) break;
                src[i] += dst[i];
            }
        }

        public static void Sum(Slice<float> dst, float value)
        {
            for (int i = 0; i < dst.Length; i++)
            {
                dst[i] -= value;
            }
        }


        public static void UpsampleLinear(Slice<float> x, Slice<float> dest, int multiple)
        {
            for (int i = x.Length; i > 0; i--)
            {
                int a = ((i - 1) * multiple);
                int b = i * multiple;
                for (int j = a; j < b; j++)
                {
                    if (j > dest.Length) break;

                    float t = (j - a) / (float)(b - a);
                    float val = QuickMafs.Lerp(x[i - 1], x[i], t);
                    dest[j] = val;
                }
            }
        }

        public static void Abs(Slice<float> x)
        {
            for (int i = 0; i < x.Length; i++)
            {
                x[i] = Math.Abs(x[i]);
            }
        }


        public static int ArgMax(Slice<float> x, bool abs = false)
        {
            int max = 0;
            for (int i = 1; i < x.Length; i++)
            {
                float xi = x[i];
                if (abs) xi = Math.Abs(xi);
                if (xi > x[max]) max = i;
            }
            return max;
        }

        public static float Max(Slice<float> x, bool abs = false)
        {
            return x[ArgMax(x, abs)];
        }

        public static int ArgMin(Slice<float> x, bool abs = false)
        {
            int min = 0;
            for (int i = 1; i < x.Length; i++)
            {
                float xi = x[i];
                if (abs) xi = Math.Abs(xi);
                if (xi < x[min]) min = i;
            }
            return min;
        }

        public static float Min(Slice<float> x, bool abs = false)
        {
            return x[ArgMin(x, abs)];
        }

        public static float Average(Slice<float> x, bool abs = false)
        {
            float average = 0;
            for (int i = 1; i < x.Length; i++)
            {
                float xi = x[i];
                if (abs) xi = Math.Abs(xi);
                average += x[i] / (float)x.Length;
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
        public static void SlidingWindowAverage(Slice<float> x, Slice<float> dest, int size, float exponent)
        {
			int newSize = x.Length - size - 1;
			if(dest.Length != newSize) return;

            float sum = 0;
            for (int i = 0; i < size; i++)
            {
                sum += x[i];
            }

            for (int i = size; i < x.Length - size; i++)
            {
                float temp = x[i - size];
                dest[i-size] = (float)Math.Pow(sum, exponent) / (float)size;
                sum -= temp;
                sum += x[i];
            }
        }

        public static void Differentiate(Slice<float> x, int len, float notchesPerUnit)
        {
            for (int i = 0; i < len - 1; i++)
            {
                float x2 = x[i + 1];
                float x1 = x[i];
                float dX = x2 - x1;
                x[i] = notchesPerUnit * dX;
            }
        }

        public static void Divide(Slice<float> x, float value)
        {
            for (int i = 1; i < x.Length - 1; i++)
            {
                x[i] = x[i] / value;
            }
        }

        public static void Normalize(Slice<float> x)
        {
            float max = Max(x, true);
            Divide(x, max);
        }

        public static void OnePoleLPF(Slice<float> x, float a1, float b0 = 1)
        {
            for (int i = 1; i < x.Length; i++)
            {
                x[i] = b0 * x[i] - a1 * x[i - 1];
            }
        }

    }
}
