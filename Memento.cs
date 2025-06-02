namespace DVG.Core
{
    public readonly struct Memento<T> : IMemento
        where T : IMementoData
    {
        public int Tick { get; }
        public int EntityId { get; }
        public T MementoData { get; }
    }
}
