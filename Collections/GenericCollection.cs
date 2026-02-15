using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace DVG.Collections
{
    public sealed class GenericCollection : IDisposable
    {
        private struct Entry
        {
            public GCHandle? Handle;
            public int Offset;
            public bool Exists;
        }

        private byte[] _buffer;
        private int _used;

        private readonly Dictionary<Type, Entry> _entries = new();
        private readonly List<Type> _keysCache = new();

        public GenericCollection(int capacity = 256)
        {
            _buffer = new byte[capacity];
        }

        ~GenericCollection()
        {
            Dispose();
        }

        public void Add<T>(T obj)
        {
            var type = typeof(T);

            if (RuntimeHelpers.IsReferenceOrContainsReferences<T>())
            {
                if (_entries.TryGetValue(type, out var existing) &&
                    existing.Exists &&
                    existing.Handle.HasValue)
                    existing.Handle.Value.Free();

                var handle = GCHandle.Alloc(obj!, GCHandleType.Normal);

                _entries[type] = new Entry
                {
                    Handle = handle,
                    Exists = true
                };
                return;
            }

            int size = Unsafe.SizeOf<T>();

            if (_entries.TryGetValue(type, out var entry))
            {
                if (!entry.Exists)
                {
                    ref byte dst = ref _buffer[entry.Offset];
                    Unsafe.WriteUnaligned(ref dst, obj);

                    entry.Exists = true;
                    _entries[type] = entry;
                    return;
                }
                ref byte rewrite = ref _buffer[entry.Offset];
                Unsafe.WriteUnaligned(ref rewrite, obj);
                return;
            }

            EnsureCapacity(_used + size);
            int offset = _used;
            ref byte newDst = ref _buffer[offset];
            Unsafe.WriteUnaligned(ref newDst, obj);

            _entries[type] = new Entry
            {
                Offset = offset,
                Exists = true
            };

            _used += size;
        }

        public bool TryGet<T>(out T obj)
        {
            if (_entries.TryGetValue(typeof(T), out var entry) && entry.Exists)
            {
                if (entry.Handle.HasValue)
                {
                    obj = (T)entry.Handle.Value.Target!;
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

            if (entry.Handle.HasValue)
                entry.Handle.Value.Free();

            entry.Exists = false;
            entry.Handle = null;
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
            _keysCache.Clear();
            _keysCache.AddRange(_entries.Keys);
            foreach (var key in _keysCache)
            {
                var entry = _entries[key];
                if (entry.Handle.HasValue && entry.Exists)
                    entry.Handle.Value.Free();
                entry.Handle = null;
                entry.Exists = false;
                _entries[key] = entry;
            }
        }

        public void Dispose()
        {
            _used = 0;
            foreach (var entry in _entries.Values)
            {
                if (entry.Handle.HasValue && entry.Exists)
                    entry.Handle.Value.Free();
            }

            _entries.Clear();
        }
    }
}