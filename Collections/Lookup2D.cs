using System;
using System.Runtime.CompilerServices;

namespace DVG.Core.Collections
{
    public sealed class Lookup2D<T> where T : struct
    {
        private T?[] _items;

        private int _width;
        private int _height;

        private int _offsetX;
        private int _offsetY;

        public int Width => _width;
        public int Height => _height;

        public int OffsetX => _offsetX;
        public int OffsetY => _offsetY;

        public Lookup2D(int width = 16, int height = 16)
        {
            if (width <= 0 || height <= 0)
                throw new ArgumentOutOfRangeException();

            _width = width;
            _height = height;

            _offsetX = width >> 1;
            _offsetY = height >> 1;

            _items = new T?[width * height];
        }

        public T this[int x, int y]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Get(x, y);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set => Set(x, y, value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Has(int x, int y)
        {
            if (!TryToIndex(x, y, out int index))
                return false;

            return _items[index].HasValue;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryGetValue(int x, int y, out T value)
        {
            if (TryToIndex(x, y, out int index) && _items[index].HasValue)
            {
                value = _items[index]!.Value;
                return true;
            }

            value = default;
            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Remove(int x, int y)
        {
            if (!TryToIndex(x, y, out int index) || !_items[index].HasValue)
                return false;

            _items[index] = null;
            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Clear()
        {
            Array.Clear(_items, 0, _items.Length);
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private T Get(int x, int y)
        {
            return _items[ToIndex(x, y)]!.Value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void Set(int x, int y, T value)
        {
            EnsureCapacity(x, y);
            _items[ToIndex(x, y)] = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int ToIndex(int x, int y)
        {
            int ix = x + _offsetX;
            int iy = y + _offsetY;
            return ix + iy * _width;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool TryToIndex(int x, int y, out int index)
        {
            int ix = x + _offsetX;
            int iy = y + _offsetY;

            if ((uint)ix >= (uint)_width ||
                (uint)iy >= (uint)_height)
            {
                index = 0;
                return false;
            }

            index = ix + iy * _width;
            return true;
        }

        private void EnsureCapacity(int x, int y)
        {
            int ix = x + _offsetX;
            int iy = y + _offsetY;

            if ((uint)ix < (uint)_width &&
                (uint)iy < (uint)_height)
                return;

            int newWidth = _width;
            int newHeight = _height;

            int minX = Math.Min(ix, 0);
            int minY = Math.Min(iy, 0);
            int maxX = Math.Max(ix, _width - 1);
            int maxY = Math.Max(iy, _height - 1);

            while (minX < 0 || maxX >= newWidth)
            {
                newWidth <<= 1;
                _offsetX += newWidth >> 2;
                minX += newWidth >> 2;
                maxX += newWidth >> 2;
            }

            while (minY < 0 || maxY >= newHeight)
            {
                newHeight <<= 1;
                _offsetY += newHeight >> 2;
                minY += newHeight >> 2;
                maxY += newHeight >> 2;
            }

            Resize(newWidth, newHeight);
        }

        private void Resize(int newWidth, int newHeight)
        {
            var newItems = new T?[newWidth * newHeight];

            for (int y = 0; y < _height; y++)
            {
                Array.Copy(
                    _items,
                    y * _width,
                    newItems,
                    (y + (_offsetY - (_height >> 1))) * newWidth + (_offsetX - (_width >> 1)),
                    _width
                );
            }

            _items = newItems;
            _width = newWidth;
            _height = newHeight;
        }
    }
}

