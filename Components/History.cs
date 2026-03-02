using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

namespace DVG.Components
{
    public struct History<T> where T : struct
    {
        public readonly T?[] _values;
        private readonly int[] _ticks;

        private readonly int _mask;

        private int _head;
        private int _count;

        [IgnoreDataMember]
        public readonly int Capacity => _values.Length;

        public History(int capacity)
        {
            if (!((capacity & (capacity - 1)) == 0))
                throw new ArgumentException("History size should be power of two");

            _values = new T?[capacity];
            _ticks = new int[capacity];

            _mask = capacity - 1;
            _head = 0;
            _count = 0;
        }

        public T? this[int tick]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            readonly get => Get(tick);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set => Set(tick, value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Set(int tick, T? value)
        {
            Debug.Assert(RollbackAssertion(tick), "Rollback called implicitly");

            if (_count == Capacity)
            {
                _head = Inc(_head);
                _count--;
            }

            int index = TailIndex();

            _ticks[index] = tick;
            _values[index] = value;

            _count++;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly T? Get(int tick)
        {
            int count = _count;
            if (count == 0)
                throw new IndexOutOfRangeException($"History does not contain tick {tick}, probably too far away in time");

            for (int i = count - 1; i >= 0; i--)
            {
                int idx = Index(i);
                if (_ticks[idx] <= tick)
                    return _values[idx];
            }

            throw new IndexOutOfRangeException($"History does not contain tick {tick}, probably too far away in time");
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Rollback(int toTick)
        {
            int count = _count;
            if (count == 0)
                return;

            while (count > 0 && GetTick(count - 1) > toTick)
                count--;

            _count = count;

            if (count == 0)
                _head = 0;
        }

        private bool RollbackAssertion(int toTick)
        {
            if (_count == 0 && toTick < GetTick(_count - 1))
            {
                Rollback(toTick);
                return false;
            }
            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private readonly int TailIndex() => (_head + _count) & _mask;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private readonly int Index(int index) => (_head + index) & _mask;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private readonly int Inc(int value) => (value + 1) & _mask;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private readonly int GetTick(int tick) => _ticks[Index(tick)];
    }
}