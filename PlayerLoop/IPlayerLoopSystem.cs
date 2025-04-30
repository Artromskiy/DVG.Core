namespace DVG.Core
{
    public interface IPlayerLoopSystem
    {
        public void Add(IPlayerLoopItem item);
        public void Remove(IPlayerLoopItem item);

        public void Start();
        public void Tick();
        public void FixedTick();
    }
}
