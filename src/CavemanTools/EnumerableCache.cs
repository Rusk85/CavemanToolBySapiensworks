using System.Linq;


namespace CavemanTools
{
    public class EnumerableCache<T>
    {
        private readonly int _size;
        private T[] _data;
        private int _i;

        public EnumerableCache(int size=5)
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