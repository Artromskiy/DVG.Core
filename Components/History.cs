using System.Runtime.CompilerServices;

namespace DVG.Components
{
    public readonly struct History<T> where T : struct
    {
        private readonly T?[] _data;
        public ref T? this[int wrapTick]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => ref _data[wrapTick];
        }
    }
}