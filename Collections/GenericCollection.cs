using DVG.Core.Collections;
using System;
using System.Runtime.CompilerServices;

namespace DVG.Collections
{
    public sealed class GenericCollection : IDisposable
    {
        private static int _nextTypeId;

        private int _usedEntries;
        private int _usedBytes;
        private byte[] _buffer;

        private readonly Lookup<Entry> _entries = new();
        private readonly Lookup<int> _typeToEntry = new();

        public GenericCollection(int capacity = 256)
        {
            _buffer = new byte[capacity];
        }

        private int GetOrCreateEntryId<T>()
        {
            var typeId = GenericTypeId<T>.TypeId;
            if (!_typeToEntry.TryGetValue(typeId, out var entryId))
                _typeToEntry[typeId] = entryId = _usedEntries++;
            return entryId;
        }

        private bool TryGetEntryId<T>(out int entryId)
        {
            return _typeToEntry.TryGetValue(GenericTypeId<T>.TypeId, out entryId);
        }

        public void Add<T>(T value)
        {
            int id = GetOrCreateEntryId<T>();
            ref var entry = ref _entries.GetOrAddRef(id);

            if (!RuntimeHelpers.IsReferenceOrContainsReferences<T>())
            {
                if (!entry.Exists)
                {
                    int size = Unsafe.SizeOf<T>();
                    EnsureCapacity(_usedBytes + size);

                    entry.Offset = _usedBytes;
                    _usedBytes += size;
                }

                ref byte dst = ref _buffer[entry.Offset];
                Unsafe.WriteUnaligned(ref dst, value);
            }
            else
            {
                entry.Reference = value;
            }

            entry.Exists = true;
        }

        // ========================= GET =========================

        public bool TryGet<T>(out T value)
        {
            if (!TryGetEntryId<T>(out var id) ||
                !_entries.TryGetValue(id, out var entry) ||
                !entry.Exists)
            {
                value = default!;
                return false;
            }

            if (!RuntimeHelpers.IsReferenceOrContainsReferences<T>())
            {
                ref byte src = ref _buffer[entry.Offset];
                value = Unsafe.ReadUnaligned<T>(ref src);
            }
            else
            {
                value = (T)entry.Reference!;
            }

            return true;
        }

        public bool Has<T>()
        {
            return TryGetEntryId<T>(out var id)
                && _entries.TryGetValue(id, out var entry)
                && entry.Exists;
        }

        public void Remove<T>()
        {
            if (TryGetEntryId<T>(out var id) &&
                _entries.TryGetValue(id, out var entry))
            {
                entry.Exists = false;
                entry.Reference = null;
                _entries[id] = entry;
            }
        }

        public void Clear()
        {
            for (int i = 0; i < _usedEntries; i++)
            {
                if (!_entries.TryGetValue(i, out var e))
                    continue;

                e.Exists = false;
                e.Reference = null;
                _entries[i] = e;
            }
        }

        public void Dispose()
        {
            _usedEntries = 0;
            _usedBytes = 0;
            _entries.Clear();
            _typeToEntry.Clear();
        }

        private void EnsureCapacity(int required)
        {
            if (required <= _buffer.Length)
                return;

            Array.Resize(ref _buffer, Math.Max(required, _buffer.Length * 2));
        }

        private struct Entry
        {
            public object? Reference;
            public int Offset;
            public bool Exists;
        }

        private static class GenericTypeId<T>
        {
            public static readonly int TypeId = _nextTypeId++;
        }
    }
}
