using System;
using System.Runtime.CompilerServices;

namespace DVG.Core.Collections
{
    public sealed class Lookup<T> where T : struct
    {
        private T?[] _items;

        public Lookup(int initialCapacity = 16)
        {
            _items = new T?[initialCapacity];
        }

        public T this[int id]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Get(id);
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set => Set(id, value);
        }

        public bool ContainsKey(int id) => Has(id);
        public bool TryGetValue(int id, out T value)
        {
            bool has = Has(id);
            value = has ? Get(id) : default;
            return has;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private T Get(int id) => _items[id]!.Value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void Set(int id, T value)
        {
            EnsureCapacity(id);
            _items[id] = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Has(int id) => (uint)id < (uint)_items.Length && _items[id].HasValue;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Remove(int id)
        {
            if (!Has(id))
                return false;

            _items[id] = null;
            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Clear()
        {
            Array.Clear(_items, 0, _items.Length);
        }

        private void EnsureCapacity(int capacity)
        {
            if (capacity < 0)
                throw new ArgumentOutOfRangeException(nameof(capacity));

            if (capacity < _items.Length)
                return;

            var newSize = (capacity + 1) << 1;

            Array.Resize(ref _items, newSize);
        }
    }
}
