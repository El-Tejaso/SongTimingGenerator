using System;

namespace SongBPMFinder
{
    /// <summary>
    /// Does math on float spans AKA Span&lt;float&gt;.
    /// 
    /// The code should be written with performance in mind.
    /// If a function is allocating things, I will try to document this and make it apparent in the code 
    /// </summary>
    public static class MathUtilSpanF
    {
        public static void Add(Span<float> a, Span<float> b, Span<float> dst)
        {
            SpanFunctional.Map(a, b, dst, (float x1, float x2) => {
                return x1 + x2;
            });
        }

        public static void Subtract(Span<float> a, Span<float> b, Span<float> dst)
        {
            SpanFunctional.Map(a, b, dst, (float x1, float x2) => {
                return x1 - x2;
            });
        }

        public static void Multiply(Span<float> a, Span<float> b, Span<float> dst)
        {
            SpanFunctional.Map(a, b, dst, (float x1, float x2) => {
                return x1 * x2;
            });
        }

        public static void Divide(Span<float> a, Span<float> b, Span<float> dst)
        {
            SpanFunctional.Map(a, b, dst, (float x1, float x2) => {
                return x1 / x2;
            });
        }


        public static void Add(Span<float> a, float scalar, Span<float> dst)
        {
            SpanFunctional.Map(a, scalar, dst, (float x1, float x2) => {
                return x1 + x2;
            });
        }

        public static void Subtract(Span<float> a, float scalar, Span<float> dst)
        {
            SpanFunctional.Map(a, scalar, dst, (float x1, float x2) => {
                return x1 - x2;
            });
        }

        public static void Multiply(Span<float> a, float scalar, Span<float> dst)
        {
            SpanFunctional.Map(a, scalar, dst, (float x1, float x2) => {
                return x1 * x2;
            });
        }

        public static void Divide(Span<float> a, float scalar, Span<float> dst)
        {
            SpanFunctional.Map(a, scalar, dst, (float x1, float x2) => {
                return x1 / x2;
            });
        }

        public static float Sum(Span<float> input)
        {
            return Sum(input, SpanFunctional.None);
        }

        public static float Sum(Span<float> input, Func<float, float> op)
        {
            return SpanFunctional.Reduce(input, 0, (float acc, float x) => {
                return acc + x;
            }, op);
        }

        public static float Average(Span<float> input)
        {
            return Sum(input) / (float)input.Length;
        }

        public static float Mean(Span<float> input, Func<float, float> op)
        {
            return Sum(input, op) / (float)input.Length;
        }

        public static float Max(Span<float> input)
        {
            return Max(input, SpanFunctional.None);
        }

        public static float Max(Span<float> input, Func<float, float> op)
        {
            return SpanFunctional.Reduce(input, float.NegativeInfinity, (float acc, float x) => {
                if (acc > x)
                    return acc;
                return x;
            }, op);
        }

        public static float Min(Span<float> input)
        {
            return Min(input, SpanFunctional.None);
        }

        public static float Min(Span<float> input, Func<float, float> op)
        {
            return SpanFunctional.Reduce(input, float.NegativeInfinity, (float acc, float x) => {
                if (acc < x)
                    return acc;
                return x;
            }, op);
        }


        public static float Variance(Span<float> input, Func<float, float> op)
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

        public static float StandardDeviation(Span<float> input)
        {
            return StandardDeviation(input, SpanFunctional.None);
        }

        public static float StandardDeviation(Span<float> input, Func<float, float> op)
        {
            return (float)Math.Sqrt(Variance(input, op));
        }


        /// <summary>
        /// Deep-copies X.
        /// Also allocates a new array for the arguments.
        /// Currently implemented with Array.Sort(keys, items)
        /// </summary>
        public static int[] ArgSortNotInPlace(Span<float> input)
        {
            int[] args = new int[input.Length];
            for(int i = 0; i < args.Length; i++)
            {
                args[i] = i;
            }

            float[] deepCopyBuffer = new float[args.Length];
            input.CopyTo(deepCopyBuffer);

            Array.Sort(deepCopyBuffer, args);

            return args;
        }
    }
}