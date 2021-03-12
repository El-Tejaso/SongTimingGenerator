using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SongBPMFinder.Audio.Timing
{
    // convenience class for working with arrays in place to reduce memory allocation/deallocation
    // It is inspired by the way python deals with arrays internally (or at least this is how I think it does)
	// Something unique about this is that it can be given a stride into an existing array.
	// This might make debugging hard, but it makes extracting and transforming soley the 
	// right channel from an audio signal in-place very easy
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

        //Mainly for debugging purposes, but do have their convenient uses
        public int InternalStart => toIdx(0);
		public int InternalEnd => toIdx(len);
		public int InternalStride => stride;

		int toIdx(int i){
			return start + i*stride;
		}

        public T this[int index] {
            get {
				int finalIndex = toIdx(index);
                if (finalIndex >= Length)
                {
                    //breakpoint
                }

                if (finalIndex >= array.Length)
                {
                    //breakpoint
                }

                return array[finalIndex];
            }
            set { array[toIdx(index)] = value; }
        }

        public Slice(T[] arr)
        {
            array = arr;
            this.start = 0;
            this.len = arr.Length;
			this.stride = 1;
        }

        public Slice<T> GetSlice(int start, int end, int newStride = 1)
        {
            if (end*stride > array.Length)
            {
                //Breakpoint
            }

			if (start*stride > array.Length)
            {
                //Breakpoint
            }

            return new Slice<T>(
				array, 
				toIdx(start), 
				toIdx(end), 
				stride*newStride
			);
        }

        public Slice(T[] arr, int start, int end, int stride = 1)
        {
            array = arr;
            this.start = start;
            this.len = (end - start)/stride;
			this.stride = stride;
        }

        public Slice<T> DeepCopy()
        {
            T[] arr = new T[Length];
            return DeepCopy(arr);
        }

        public Slice<T> DeepCopy(T[] buffer)
        {
            for (int i = 0; i < Length; i++)
            {
                buffer[i] = array[toIdx(i)];
            }

            return new Slice<T>(buffer);
        }
    }

}
