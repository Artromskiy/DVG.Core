using System;

namespace DVG.Components
{
    public readonly struct History<T> where T : struct
    {
        private readonly T?[] _data;
        private readonly int[] _ticks;

        public History(int length)
        {
            _data = new T?[length];
            _ticks = new int[length];
        }

        public T? this[int tick]
        {
            get
            {
                int searchTick = tick;
                int limit = tick - Constants.MaxHistoryTicks;
                int newest = -1;

                while (searchTick >= limit)
                {
                    var wrappedSearch = Constants.WrapTick(searchTick);
                    var writtenTick = _ticks[wrappedSearch];
                    if (writtenTick <= searchTick)
                    {
                        if (writtenTick == searchTick)
                            return _data[wrappedSearch];
                        if (writtenTick < searchTick && writtenTick > newest)
                            newest = writtenTick;
                    }
                    searchTick--;
                }
                if (newest != -1)
                    return _data[Constants.WrapTick(newest)];
                throw new IndexOutOfRangeException();
            }
            set
            {
                int index = Constants.WrapTick(tick);
                int next = Constants.WrapTick(tick + 1);
                if (_ticks[next] > _ticks[index])
                {
                    _ticks[next] = _ticks[index] + 1;
                    _data[next] = _data[index];
                }
                _data[index] = value;
                _ticks[index] = tick;
            }
        }

        public void Rollback(int toTick)
        {
            for (int i = 0; i <= Constants.MaxHistoryTicks; i++)
            {
                int index = Constants.WrapTick(i);

                if (_ticks[index] > toTick)
                {
                    _ticks[index] = 0;
                    _data[index] = null;
                }
            }
        }
    }
}