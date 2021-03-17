using SongBPMFinder.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SongBPMFinder.Util
{
    /// <summary>
    /// A static class for performing operations on large arrays in-place.
    /// As we can be working with GB-size arrays, we should perform as few copys as possible
    /// </summary>
    class SliceMathf
    {
        // This implementation was taken from the Accord.Net framework and adapted to use floats as well as allocate less memory
        // https://github.com/accord-net/framework/blob/development/Sources/Accord.Math/Wavelets/Haar.cs
		// The temp buffer is manually specified to reduce memory allocations
        static void haarFWTSingle(Slice<float> data, Slice<float> dst)
        {
            float w0 = 0.5f;
            float w1 = -0.5f;
            float s0 = 0.5f;
            float s1 = 0.5f;

            int h = data.Length / 2;

            for (int i = 0; i < h; i++)
            {
                int k = (i * 2);
                float dK = data[k];
                float dK1 = data[k+1];
                dst[i] = dK * s0 + dK1 * s1;
                dst[i + h] = dK * w0 + dK1 * w1;
            }
        }

		public static Slice<float>[] GetDetailCoefficients(Slice<float>[] dwtSlices)
		{
			Slice<float>[] newSlices = new Slice<float>[dwtSlices.Length];
			Array.Copy(dwtSlices, 0, newSlices, 0, dwtSlices.Length);

			//Resize DWT slices so that they only keep their detail coefficients
            for (int i = 0; i < dwtSlices.Length; i++)
            {
                newSlices[i] = dwtSlices[i].GetSlice(dwtSlices[i].Length / 2, dwtSlices[i].Length);
            }
			return newSlices;
		}

		//Performs a Haar DWT in the data numLevels times, and returns slices corresponding to each segment
        //The slices will index into the data buffer as well as the data array
		public static Slice<float>[] HaarFWT(Slice<float> data, Slice<float> dataBuffer, int numLevels) {
            Slice<float>[] outSlices = new Slice<float>[numLevels];

            for (int i = 0, h = data.Length; i < numLevels; i++, h /= 2)
            {
                Slice<float> inSlice = data.GetSlice(0, h);
                outSlices[i] = dataBuffer.GetSlice(0, h);

                haarFWTSingle(inSlice, dataBuffer);

                Slice<float> temp = data;
                data = dataBuffer;
                dataBuffer = temp;
            }

			outSlices = SliceMathf.GetDetailCoefficients(outSlices);

			return outSlices;
		}

        public static Slice<float> ZeroesLike(Slice<float> x)
        {
            return new Slice<float>(new float[x.Length]);
        }

		public static Slice<float>[] DownsampleCoefficients(Slice<float>[] dwtSlices, Slice<float> original, int mainDownsampleFactor)
        {
			int numLevels = dwtSlices.Length;

            Slice<float>[] downsampleSlices = new Slice<float>[numLevels];
            int sliceLen = (original.Length / mainDownsampleFactor);

            for (int i = 0; i < numLevels; i++)
            {
                downsampleSlices[i] = original.GetSlice(i * sliceLen, (i + 1) * sliceLen);

                //dwtSlice[level] should have a length of original.Length / level
                int downsampleFactor = mainDownsampleFactor / (original.Length/dwtSlices[numLevels - i - 1].Length);
				
                SliceMathf.DownsampleAverage(dwtSlices[numLevels - i - 1], downsampleSlices[i], downsampleFactor);
            }

			return downsampleSlices;
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

        public static void DownsampleMax(Slice<float> x, Slice<float> dst, int samples)
        {
            for (int i = 0; i < dst.Length; i++)
            {
                float max = 0;
                for (int j = 0; j < samples; j++)
                {
                    if ((i * samples + j) > x.Length)
                        break;
                    max = Math.Max(max, x[i * samples + j]);
                }

                dst[i] = max;
            }
        }

		public static float Sum(Slice<float> x, bool abs = false)
        {
			float sum = 0;
            for(int i = 0; i < x.Length; i++)
            {
				float xi = x[i];
				if(abs) xi = (float)Math.Abs(xi);
                sum += xi;
            }
			return sum;
        }

        public static void Sum(Slice<float> x, Slice<float> other)
        {
            for(int i = 0; i < x.Length; i++)
            {
                if (i > other.Length) break;
                x[i] += other[i];
            }
        }

        public static void Sum(Slice<float> dst, float value)
        {
            for (int i = 0; i < dst.Length; i++)
            {
                dst[i] += value;
            }
        }

        public static void Max(Slice<float> dst, float value)
        {
            for (int i = 0; i < dst.Length; i++)
            {
                if(value > dst[i]) dst[i] = value;
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
                if (x[i] < 0) x[i] = -x[i];
            }
        }


        public static int ArgMax(Slice<float> x, bool abs = false)
        {
            int maxIndex = 0;

            float max = x[0];
            if (abs) max = Math.Abs(max);

            for (int i = 1; i < x.Length; i++)
            {
                float xi = x[i];
                if (abs) xi = Math.Abs(xi);

                if (xi > max)
                {
                    max = xi;
                    maxIndex = i;
                }
            }

            return maxIndex;
        }

        public static float Max(Slice<float> x, bool abs = false)
        {
            if(abs) return Math.Abs(x[ArgMax(x, true)]);
            return x[ArgMax(x, false)];
        }

        public static int ArgMin(Slice<float> x, bool abs = false)
        {
            int minIndex = 0;

            float min = x[0];
            if (abs) min = Math.Abs(min);

            for (int i = 1; i < x.Length; i++)
            {
                float xi = x[i];
                if (abs) xi = Math.Abs(xi);

                if (xi < min)
                {
                    min = xi;
                    minIndex = i;
                }
            }

            return minIndex;
        }

        public static float Min(Slice<float> x, bool abs = false)
        {
            int minIndex = 0;

            float min = x[0];
            if (abs) min = Math.Abs(min);

            for (int i = 1; i < x.Length; i++)
            {
                float xi = x[i];
                if (abs) xi = Math.Abs(xi);

                if (xi < min)
                {
                    min = xi;
                    minIndex = i;
                }
            }

            return minIndex;
        }

        public static float Average(Slice<float> x, bool abs = false)
        {
            return Sum(x,abs)/(float)x.Length;
        }

		public static float VarianceSum(Slice<float> x){
            float variance = 0;
            float mean = Average(x, false);

            for (int i = 0; i < x.Length; i++)
            {
                float xi = x[i];

                variance += ((xi - mean) * (xi - mean));
            }

			return variance;
		}

        public static float StdDev(Slice<float> x)
        {
            return (float)Math.Sqrt(VarianceSum(x)/(float)x.Length);
        }

        /// <summary>
        /// Generates a Sliding window average in place.
        /// Returns the new length of the array
        /// </summary>
        /// <param name="x">The array to operate on. Can overlap with dest</param>
        /// <param name="dest">the destination array, ideally of size x.Length - size. Can overlap with x</param>
        /// <param name="size">The size of the window</param>
		/// <param name="exponent">The exponent to use</param>
        public static void SlidingWindowAverage(Slice<float> x, Slice<float> dest, int size, float exponent)
        {
			int newSize = x.Length - size - 1;
			if(dest.Length != newSize) return;

            float sum = 0;
            for (int i = 0; i < size; i++)
            {
                sum += x[i];
            }

            for (int i = size; i <= x.Length - size; i++)
            {
                float temp = x[i - size];
                dest[i-size] = (float)Math.Pow(sum, exponent) / (float)size;
                sum -= temp;
                sum += x[i];
            }
        }

        public static void Differentiate(Slice<float> x, float OneOverDt)
        {
            for (int i = 0; i < x.Length - 1; i++)
            {
                float x2 = x[i + 1];
                float x1 = x[i];
                float dX = x2 - x1;
                x[i] = dX * OneOverDt;
            }
        }

        /// <summary>
        /// Performs autocorellation.
        /// it does this with wrapping.
        /// dst and src must not overlap.
        /// </summary>
        /// <param name="src">must be the same length as dst, and musn't overlap</param>
        /// <param name="dst">must be the same length as src, and musn't overlap</param>
        public static void Autocorrelate(Slice<float> src, Slice<float> dst)
        {
            if(src.Length != dst.Length)
            {
                //bruh
                return;
            }

			float mean = Average(src, false);
			float varianceSum = VarianceSum(src);

            //Autocorrelation
            //This operation is O(N^2), potentially a bottleneck, which explains the windowed approach used by others
            int n = dst.Length;
            for (int i = 0; i < n; i++)
            {
                float sum = 0;

                for (int j = 0; j < n; j++)
                {
                    sum += (src[i]) * (src[(i + j) % src.Length]);
                }

                dst[i] = sum / (float)n;
            }
        }

        public static void Divide(Slice<float> x, float value)
        {
			Mult(x, 1.0f/value);
        }
		
		public static void Mult(Slice<float> x, float value)
        {
			for (int i = 0; i < x.Length; i++)
            {
                x[i] = x[i] * value;
            }
        }

        public static void Normalize(Slice<float> x)
        {
            float max = Max(x, true);
            Divide(x, max);
        }

		public static void NormalizeMeanStdDev(Slice<float> x){
			//Subtract means
			float average = Average(x, false);
			Sum(x, -average);

			float stdev = StdDev(x);
			Mult(x, 1.0f/stdev);
		}

        public static void NormalizeAv(Slice<float> x)
        {
            float av= Average(x, true);
            Divide(x, av);
        }


        public static void OnePoleLPF(Slice<float> x, float a1, float b0 = 1)
        {
            for (int i = 1; i < x.Length; i++)
            {
                x[i] = b0 * x[i] - a1 * x[i - 1];
            }
        }

        //Taken from https://github.com/accord-net/framework/blob/development/Sources/Accord.Audio/Filters/HighPassFilter.cs
        //and adapted
        public static void HighPassFilter(Slice<float> src, double frequency, double sampleRate)
        {
            double rc = 1 / (2 * Math.PI * frequency);
            double dt = 1 / (sampleRate);

            float alpha = (float)(rc / (rc + dt));

            // memorize the previous sample
            float x0 = src[0];

            for (int i = 1; i < src.Length; i++)
            {
                float temp = src[i];
                src[i] = alpha * (src[i-1] + src[i] - x0);
                x0 = temp;
            }
        }


        //Taken from Accord.Audio same as HighPassFilter but LowPassFilter
        //and adapted
        public static void LowPassFilter(Slice<float> src, double frequency, double sampleRate)
        {
            double rc = 1 / (2 * Math.PI * frequency);
            double dt = 1 / (sampleRate);

            float alpha = (float)(rc / (rc + dt));

            for (int i = 1; i < src.Length; i++)
            {
                src[i] = src[i - 1] + alpha * (src[i] - src[i-1]);
            }
        }
    }
}
