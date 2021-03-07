using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SongBPMFinder.Audio.Timing
{
    //convenience class for working with arrays in place to reduce memory allocation/deallocation
    //It is inspired by the way python deals with arrays internally (or at least this is how I think it does)
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
            if (end > array.Length)
            {
                //Breakpoint
            }
            return new Slice<T>(array, this.start + start, this.start + end);
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
            return DeepCopy(arr);
        }

        public Slice<T> DeepCopy(T[] buffer)
        {
            for (int i = 0; i < Length; i++)
            {
                buffer[i] = array[start + i];
            }

            return new Slice<T>(buffer);
        }
    }

}
