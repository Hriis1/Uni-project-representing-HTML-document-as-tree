using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;

namespace Kursova_Rabota_1_HTML
{
    public class MyList<T> : IEnumerable<T>
    {
        private T[] _items;
        private int _size;
        private const int _growFactor = 2;

        public MyList()
        {
            _items = new T[2];
        }

        public int Count
        {
            get
            {
                return _size;
            }
        }

        public void Add(T item)
        {
            if (_size + 1 > _items.Length)
            {
                var newArray = new T[_items.Length * _growFactor];
                _items.CopyTo(newArray, 0);
                _items = newArray;
            }
            _items[_size] = item;
            _size++;
        }

        public void Remove(T item)
        {
            T[] newItems = new T[_items.Length -1];
            bool changed = false;
            for (int i = 0; i < _items.Length; i++)
            {
                if(_items[i].Equals(item) && changed == false)
                {
                    changed = true;
                }
                else
                {
                    if (changed == false)
                    {
                        if (i >= _items.Length - 1)
                            break;
                        newItems[i] = _items[i];
                    }
                    else
                    {
                        newItems[i-1] = _items[i];
                    }
                }
                
            }

           
            if (changed == true)
            {
                _items = newItems;
                _size--;
            }
        }

        public void Clear()
        {
            _items = new T[2];
            _size = 0;
        }

        public bool Contains(T item)
        {
            foreach (var value in _items)
            {
                if (value.Equals(item))
                {
                    return true;
                }
            }
            return false;
        }

        public T this[int i]
        {
            get => _items[i];
            set => _items[i] = value;
        }
        public void CopyTo(T[] array, int arrayIndex)
        {
            _items.CopyTo(array, arrayIndex);
        }

        public IEnumerator<T> GetEnumeratorGenrric()
        {
            for (var i = 0; i < _size; i++)
            {
                yield return _items[i];
            }
        }
        public IEnumerator GetEnumerator()
        {
            for (var i = 0; i < _size; i++)
            {
                yield return _items[i];
            }
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return GetEnumeratorGenrric();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

    }
}
