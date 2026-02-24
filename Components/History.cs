using System.Runtime.CompilerServices;

namespace DVG.Components
{
    [InlineArray(Constants.HistoryTicks)]
    public readonly struct History<T> where T : struct
    {
        private readonly InlineArray256<T?> _data;
        public ref T? this[int wrapTick]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => ref _data.Span[wrapTick];
        }
    }
}