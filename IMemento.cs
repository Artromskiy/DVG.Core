namespace DVG.Core
{
    public interface IMemento
    {
        public int EntityId { get; }
        public int Tick { get; }
    }
}
