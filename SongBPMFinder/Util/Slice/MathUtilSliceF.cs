using System;

namespace SongBPMFinder
{
    /// <summary>
    /// Does math on float slices AKS Slice&lt;float&gt;.
    /// 
    /// The code should be written with performance in mind.
    /// If a function is allocating things, I will try to document this and make it apparent in the code 
    /// </summary>
    public static class MathUtilSliceF
    {

        public static void Add(Slice<float> a, Slice<float> b, Slice<float> dst)
        {
            SliceFunctional.Map(a, b, dst, (float x1, float x2) => {
                return x1 + x2;
            });
        }

        public static void Subtract(Slice<float> a, Slice<float> b, Slice<float> dst)
        {
            SliceFunctional.Map(a, b, dst, (float x1, float x2) => {
                return x1 - x2;
            });
        }

        public static void Multiply(Slice<float> a, Slice<float> b, Slice<float> dst)
        {
            SliceFunctional.Map(a, b, dst, (float x1, float x2) => {
                return x1 * x2;
            });
        }

        public static void Divide(Slice<float> a, Slice<float> b, Slice<float> dst)
        {
            SliceFunctional.Map(a, b, dst, (float x1, float x2) => {
                return x1 / x2;
            });
        }

        public static float Sum(Slice<float> input)
        {
            return Sum(input, SliceFunctional.None);
        }

        public static float Sum(Slice<float> input, Func<float, float> op)
        {
            return SliceFunctional.Reduce(input, 0, (float acc, float x) => {
                return acc + x;
            }, op);
        }

        public static float Average(Slice<float> input)
        {
            return Sum(input) / (float)input.Length;
        }

        public static float Mean(Slice<float> input, Func<float, float> op)
        {
            return Sum(input, op) / (float)input.Length;
        }

        public static float Max(Slice<float> input)
        {
            return Max(input, SliceFunctional.None);
        }

        public static float Max(Slice<float> input, Func<float, float> op)
        {
            return SliceFunctional.Reduce(input, float.NegativeInfinity, (float acc, float x) => {
                if (acc > x)
                    return acc;
                return x;
            }, op);
        }

        public static float Min(Slice<float> input)
        {
            return Min(input, SliceFunctional.None);
        }

        public static float Min(Slice<float> input, Func<float, float> op)
        {
            return SliceFunctional.Reduce(input, float.NegativeInfinity, (float acc, float x) => {
                if (acc < x)
                    return acc;
                return x;
            }, op);
        }


        public static float Variance(Slice<float> input, Func<float, float> op)
        {
            float mean = Mean(input, op);

            float topPart = 0;

            //Not sure how to do this functionally without using a lambda that 
            //captures the mean local variable  and will therefore not be cached. 
            //A for-loop will be fine for now
            //TODO: figure this out
            for (int i = 0; i < input.Length; i++)
            {
                float x = input[i];
                topPart += (x - mean) * (x - mean);
            }

            return topPart / (float)input.Length;
        }

        public static float StandardDeviation(Slice<float> input)
        {
            return StandardDeviation(input, SliceFunctional.None);
        }

        public static float StandardDeviation(Slice<float> input, Func<float, float> op)
        {
            return (float)Math.Sqrt(Variance(input, op));
        }


        /// <summary>
        /// Deep-copies X.
        /// Also allocates a new array for the arguments.
        /// Currently implemented with Array.Sort(keys, items)
        /// </summary>
        public static Slice<int> DeepCopyArgSort(Slice<float> input)
        {
            Slice<int> args = new Slice<int>(new int[input.Length]);
            for(int i = 0; i < args.Length; i++)
            {
                args[i] = i;
            }

            //Doing it like this so that the it is clearer to the compiler that this array actually goes out of scope here,
            //and can be freed immediately
            float[] deepCopyBuffer = new float[args.Length];
            input.DeepCopy(deepCopyBuffer);

            Array.Sort(deepCopyBuffer, args.GetInternalArray());

            return args;
        }
    }
}