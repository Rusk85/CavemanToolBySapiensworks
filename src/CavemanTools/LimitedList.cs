using System.Linq;


namespace CavemanTools
{
    /// <summary>
    /// Used to maintain a fixed sized list for history purposes.
    /// Intended to support idempotency (by storing last n message ids)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class LimitedList<T>
    {
        private readonly int _size;
        private T[] _data;
        private int _i;

        public LimitedList(int size=5)
        {
            _size = size;
            _data = new T[size];
            _i = 0;
        }

        public void Add(T item)
        {
            if (_i == _size)
            {
                _i = 0;
            }
            _data[_i] = item;
            _i++;
        }

        public bool Contains(T item)
        {
            return _data.Contains(item);
        }
    }
}