namespace DVG.Components
{
    public readonly struct History<T> where T : struct
    {
        private readonly T?[] _data;
        public ref T? this[int wrapTick] => ref _data[wrapTick];

        public History(int length) => _data = new T?[length];
    }
}