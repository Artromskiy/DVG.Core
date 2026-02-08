using System;
using System.Runtime.CompilerServices;

namespace DVG.Core.Collections
{
    public sealed class Lookup<T> where T : struct
    {
        private T?[] _items;
        private int _offset;

        public int Length => _items.Length;
        public int Offset => _offset;

        public Lookup(int initialCapacity = 16)
        {
            if (initialCapacity <= 0)
                throw new ArgumentOutOfRangeException(nameof(initialCapacity));

            _items = new T?[initialCapacity];
            _offset = initialCapacity >> 1;
        }

        public T this[int id]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Get(id);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set => Set(id, value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool ContainsKey(int id) => Has(id);


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryGetValue(int id, out T value)
        {
            if (TryToIndex(id, out int index) && _items[index].HasValue)
            {
                value = _items[index]!.Value;
                return true;
            }

            value = default;
            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Remove(int id)
        {
            if (!TryToIndex(id, out int index) || !_items[index].HasValue)
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
        private T Get(int id) => _items[ToIndex(id)]!.Value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void Set(int id, T value)
        {
            EnsureCapacity(id);
            _items[ToIndex(id)] = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool Has(int id) => TryToIndex(id, out int index) && _items[index].HasValue;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int ToIndex(int id) => id + _offset;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool TryToIndex(int id, out int index)
        {
            index = ToIndex(id);
            return (uint)index < (uint)_items.Length;
        }

        private void EnsureCapacity(int id)
        {
            int index = id + _offset;
            if ((uint)index < (uint)_items.Length)
                return;

            int newSize = _items.Length;
            int newOffset = _offset;

            int minIndex = Math.Min(index, 0);
            int maxIndex = Math.Max(index, newSize - 1);

            while (minIndex < 0 || maxIndex >= newSize)
            {
                int grow = newSize;
                newSize <<= 1;
                newOffset += grow >> 1;
                minIndex += grow >> 1;
                maxIndex += grow >> 1;
            }

            Resize(newSize, newOffset);
        }

        private void Resize(int newSize, int newOffset)
        {
            var newItems = new T?[newSize];

            Array.Copy(
                _items,
                0,
                newItems,
                newOffset - _offset,
                _items.Length
            );

            _items = newItems;
            _offset = newOffset;
        }
    }
}
