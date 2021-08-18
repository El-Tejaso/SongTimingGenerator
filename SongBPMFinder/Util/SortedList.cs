using System;
using System.Collections.Generic;

namespace SongBPMFinder
{
    public class SortedList<T> where T : struct, IComparable<T>
    {
        List<T> _elements;

        public T this[int index] {
            get {
                return _elements[index];
            }
        }

        public int Count {
            get { return _elements.Count; }
        }

        public T[] ToArray()
        {
            return _elements.ToArray();
        }

        /// <summary>
        /// returns the index that the object was added to
        /// </summary>
        public int Add(T obj)
        {
            _elements.Add(obj);

            return ensureSorted(obj, _elements.Count - 1);
        }

        public SortedList()
        {
            _elements = new List<T>();
        }

        public SortedList(List<T> objects)
        {
            _elements = objects;
            _elements.Sort();
        }

        public int IndexOf(T obj)
        {
            return _elements.IndexOf(obj);
        }

        public void Remove(T obj)
        {
            int index = IndexOf(obj);

            if (index < 0)
                return;

            RemoveAt(index);
        }

        public void RemoveAt(int index)
        {
            _elements.RemoveAt(index);
        }


        public void Clear()
        {
            _elements.Clear();
        }

        private int ensureSorted(T obj, int index)
        {
            if (index == -1)
            {
                index = IndexOf(obj);
            }

            if (index >= 0 && index <= _elements.Count - 1)
            {
                bool shouldBeMovedDown = index > 0 && (_elements[index].CompareTo(_elements[index - 1]) < 0);
                bool shouldBeMovedUp = index < _elements.Count - 1 && (_elements[index].CompareTo(_elements[index + 1]) > 0);

                //It is possible to do this with just one function but I cant be arsed
                if (shouldBeMovedDown)
                {
                    return moveDown(index);
                }
                else if (shouldBeMovedUp)
                {
                    return moveUp(index);
                }
            }

            return index;
        }

        private int moveDown(int index)
        {
            for (int i = index; i > 0; i--)
            {
                if (_elements[i].CompareTo(_elements[i - 1]) < 0)
                {
                    var temp = _elements[i];
                    _elements[i] = _elements[i - 1];
                    _elements[i - 1] = temp;
                }
                else
                {
                    return i;
                }
            }

            return 0;
        }

        private int moveUp(int index)
        {
            for (int i = index; i < _elements.Count - 1; i++)
            {
                if (_elements[i].CompareTo(_elements[i + 1]) > 0)
                {
                    var temp = _elements[i];
                    _elements[i] = _elements[i + 1];
                    _elements[i + 1] = temp;
                }
                else
                {
                    return i;
                }

            }

            return _elements.Count - 1;
        }
    }
}
