using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace DVG.SkyPirates.Shared.Tools
{
    public sealed class GenericCollection
    {
        private byte[] _buffer;
        private int _used;

        private readonly Dictionary<Type, int> _offsets = new();
        private readonly Dictionary<Type, GCHandle> _handles = new();
        private readonly Dictionary<Type, bool> _has = new();

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
                _handles[type] = GCHandle.Alloc(obj!, GCHandleType.Normal);
                _has[type] = true;
                return;
            }

            int size = Unsafe.SizeOf<T>();
            EnsureCapacity(_used + size);

            ref byte dst = ref _buffer[_used];
            Unsafe.WriteUnaligned(ref dst, obj);

            _offsets[type] = _used;
            _has[type] = true;

            _used += size;
        }

        public bool TryGet<T>(out T obj)
        {
            var type = typeof(T);

            if (!_has.TryGetValue(type, out var exists) || !exists)
            {
                obj = default!;
                return false;
            }

            if (_handles.TryGetValue(type, out var handle))
            {
                obj = (T)handle.Target!;
                return true;
            }

            if (_offsets.TryGetValue(type, out var offset))
            {
                ref byte src = ref _buffer[offset];
                obj = Unsafe.ReadUnaligned<T>(ref src);
                return true;
            }

            obj = default!;
            return false;
        }

        public void Remove<T>()
        {
            var type = typeof(T);

            if (!_has.TryGetValue(type, out var exists) || !exists)
                return;

            if (_handles.TryGetValue(type, out var handle))
            {
                handle.Free();
                _handles.Remove(type);
            }

            _offsets.Remove(type);
            _has[type] = false;
        }

        public bool Has<T>()
        {
            return _has.TryGetValue(typeof(T), out var exists) && exists;
        }

        private void EnsureCapacity(int required)
        {
            if (required <= _buffer.Length)
                return;

            var doubled = _buffer.Length * 2;
            int newSize = required > doubled ? required : doubled;
            Array.Resize(ref _buffer, newSize);
        }

        public void Clear()
        {
            foreach (var handle in _handles.Values)
                handle.Free();

            _handles.Clear();
            _offsets.Clear();
            _has.Clear();
        }
    }
}
