namespace DVG.Core
{
    public readonly struct Memento<T> : IMemento
        where T : IMementoData
    {
        public int Tick { get; }
        public int EntityId { get; }
        public T Data { get; }

        public Memento(int tick, int entityId, T data)
        {
            Tick = tick;
            EntityId = entityId;
            Data = data;
        }
    }
}
