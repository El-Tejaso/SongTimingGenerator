﻿namespace SongBPMFinder
{
    /// <summary>
    /// A convenience class for working with arrays in place to reduce memory allocation/deallocation
    /// that ended up doing more damage than good. I will delete it and all dependancies in the next commit if I can.
    /// </summary>
    public struct Slice<T>
    {
        int start;
        int len;
        int stride;

        T[] array;

        public T[] GetInternalArray()
        {
            return array;
        }

        public int Length => len;

#if DEBUG
        /// <summary>
        /// Mainly for viewing in the IDE when debuging. Should not be used in production
        /// </summary>
        public int InternalStart => start;
        /// <summary>
        /// Mainly for viewing in the IDE when debuging. Should not be used in production
        /// </summary>
        public int InternalEnd => start + len * stride;
        /// <summary>
        /// Mainly for viewing in the IDE when debuging. Should not be used in production
        /// </summary>
        public int InternalStride => stride;
#endif

        public T this[int index] {
            get {
                return array[start + index * stride];
            }
            set { array[start + index * stride] = value; }
        }

        public Slice(T[] arr)
        {
            array = arr;
            start = 0;
            len = arr.Length;
            stride = 1;
        }

        /// <summary>
        /// start inclusive, end exclusive. 
        /// 
        /// Stride is the number of indices to step forward to the next item.
        /// Anything 1 and greater is valid. 
        /// 
        /// A common use is to quickly get the left and right channels of interleaved audio with a stride of 2
        /// 
        /// Permits indexing outside of the defined slice bounds. Make sure you know what you are doing though
        /// </summary>
        public Slice<T> GetSlice(int newStart, int newEnd, int newStride = 1)
        {
            if (newEnd * stride > array.Length)
            {
                //Breakpoint
            }

            return new Slice<T>(
                array,
                start + newStart * stride,
                start + newEnd * stride,
                stride * newStride
            );
        }

        public Slice(T[] arr, int start, int end, int stride = 1)
        {
            array = arr;
            this.start = start;
            len = (end - start) / stride;
            this.stride = stride;
        }

        public Slice<T> DeepCopy()
        {
            T[] arr = new T[len];
            return DeepCopy(arr);
        }

        public Slice<T> DeepCopy(T[] buffer)
        {
            for (int i = 0; i < len; i++)
            {
                buffer[i] = array[start + i * stride];
            }

            return new Slice<T>(buffer);
        }
    }

}