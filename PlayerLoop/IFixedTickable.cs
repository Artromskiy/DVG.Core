namespace DVG.Core
{
    public interface IFixedTickable : IPlayerLoopItem
    {
        void Tick(fix deltaTime);
    }
}
