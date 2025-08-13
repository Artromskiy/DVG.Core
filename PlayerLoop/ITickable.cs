namespace DVG.Core
{
    public interface ITickable : IPlayerLoopItem
    {
        void Tick(float deltaTime);
    }
}
