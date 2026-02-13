using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace DVG.Collections
{
    public sealed class GenericCollection
    {
        private struct Entry
        {
            public int Offset;
            public GCHandle Handle;
            public bool IsManaged;
            public bool Exists;
        }

        private byte[] _buffer;
        private int _used;

        private readonly Dictionary<Type, Entry> _entries = new();

        public GenericCollection(int capacity = 256)
        {
            _buffer = new byte[capacity];
        }

        ~GenericCollection()
        {
            Clear();
        }


        public void Add<T>(T obj)
        {
            var type = typeof(T);

            Remove<T>();

            if (RuntimeHelpers.IsReferenceOrContainsReferences<T>())
            {
                var handle = GCHandle.Alloc(obj!, GCHandleType.Normal);

                _entries[type] = new Entry
                {
                    Handle = handle,
                    IsManaged = true,
                    Exists = true
                };

                return;
            }

            int size = Unsafe.SizeOf<T>();
            EnsureCapacity(_used + size);

            ref byte dst = ref _buffer[_used];
            Unsafe.WriteUnaligned(ref dst, obj);

            _entries[type] = new Entry
            {
                Offset = _used,
                IsManaged = false,
                Exists = true
            };

            _used += size;
        }

        public bool TryGet<T>(out T obj)
        {
            if (_entries.TryGetValue(typeof(T), out var entry) && entry.Exists)
            {
                if (entry.IsManaged)
                {
                    obj = (T)entry.Handle.Target!;
                    return true;
                }

                ref byte src = ref _buffer[entry.Offset];
                obj = Unsafe.ReadUnaligned<T>(ref src);
                return true;
            }

            obj = default!;
            return false;
        }

        public void Remove<T>()
        {
            var type = typeof(T);

            if (!_entries.TryGetValue(type, out var entry) || !entry.Exists)
                return;

            if (entry.IsManaged)
                entry.Handle.Free();

            entry.Exists = false;
            _entries[type] = entry;
        }

        public bool Has<T>()
        {
            return _entries.TryGetValue(typeof(T), out var entry) && entry.Exists;
        }

        private void EnsureCapacity(int required)
        {
            if (required <= _buffer.Length)
                return;

            int newSize = Math.Max(required, _buffer.Length * 2);
            Array.Resize(ref _buffer, newSize);
        }

        public void Clear()
        {
            foreach (var entry in _entries.Values)
            {
                if (entry.IsManaged && entry.Exists)
                    entry.Handle.Free();
            }

            _entries.Clear();
        }
    }
}
