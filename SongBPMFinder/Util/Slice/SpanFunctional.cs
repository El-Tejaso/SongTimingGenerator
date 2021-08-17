using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SongBPMFinder
{
    /// <summary>
    /// An attempt at creating a (mostly) zero-allocation functional API
    /// </summary>
    public static class SpanFunctional
    {
        public static void AssertEqualLength<T1, T2, T3>(Span<T1> a, Span<T2> b, Span<T3> c)
        {
            AssertEqualLength(a, b);
            AssertEqualLength(a, c);
        }

        public static void AssertEqualLength<T1, T2>(Span<T1> a, Span<T2> b)
        {
#if DEBUG
            if (a.Length != b.Length)
                throw new Exception("The two slices must be of equal length for this operation to function");
#endif
        }

        public static void AssertASmallerThanB<T>(Span<T> a, Span<T> b, int amount)
        {
#if DEBUG
            if (a.Length + amount >= b.Length)
                throw new Exception("Slice a must be smaller than b by " + amount);
#endif
        }

        public static T None<T>(T a)
        {
            return a;
        }


        /// <summary>
        /// Check documentation for the other overload
        /// </summary>
        public static void Map<T0, T1>(Span<T0> a, Span<T0> b, Span<T1> dst, Func<T0, T0, T1> function)
        {
            Map(a, b, dst, function, None, None);
        }

        /// <summary>
        /// function is of the format f(a_i, b_i) -> dst_i
        /// 
        /// Most of the time, function will be a compile time constant, so it 
        /// can be unrolled by the into a normal for-loop with zero peformance overhead(hopefully)
        /// </summary>
        public static void Map<T0, T1>(Span<T0> a, Span<T0> b, Span<T1> dst, Func<T0, T0, T1> function, Func<T0, T0> aOp, Func<T0, T0> bOp)
        {
            AssertEqualLength(a, b, dst);

            for (int i = 0; i < dst.Length; i++)
            {
                dst[i] = function(aOp(a[i]), bOp(b[i]));
            }
        }

        /// <summary>
        /// function is of the format f(a_i, scalar) -> dst_i
        /// 
        /// Same as the other overload but applies a single scalar to all elements of a.
        /// Imagine a list the same length as a but filled with a single value, and calling Map with that.
        /// The result is the same but with way less memory usage
        /// </summary>
        public static void Map<T0, T1>(Span<T0> a, T0 scalar, Span<T1> dst, Func<T0, T0, T1> function)
        {
            Map(a, scalar, dst, function, None, None);
        }

        public static void Map<T0, T1>(Span<T0> a, T0 scalar, Span<T1> dst, Func<T0, T0, T1> function, Func<T0, T0> aOp, Func<T0, T0> bOp)
        {
            T0 s = bOp(scalar);
            for (int i = 0; i < dst.Length; i++)
            {
                dst[i] = function(aOp(a[i]), s);
            }
        }

        /// <summary>
        /// Same as the other overload, but aOp is None
        /// </summary>
        public static T Reduce<T>(Span<T> a, T defaultReturn, Func<T, T, T> function)
        {
            return Reduce(a, defaultReturn, function, None);
        }

        /// <summary>
        /// function is of the format f(accumulator_i-1, x_i) -> accumulator_i.
        /// the initial value of the accumulator is a[0].
        /// 
        /// if a is empty, something needs to be returned, which is specified with defaultReturn
        /// </summary>
        public static T Reduce<T>(Span<T> a, T defaultReturn, Func<T, T, T> function, Func<T, T> aOp)
        {
            if (a.Length == 0)
                return defaultReturn;

            T accumulator = aOp(a[0]);
            for (int i = 1; i < a.Length; i++)
            {
                accumulator = function(accumulator, aOp(a[i]));
            }
            return accumulator;
        }
    }
}
