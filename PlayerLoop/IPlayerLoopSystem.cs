using System;

namespace DVG.Core
{
    public interface IPlayerLoopSystem : IDisposable
    {
        public event Action<Exception> ExceptionHandler;
        public void Add(IPlayerLoopItem item);
        public void Remove(IPlayerLoopItem item);
            
        public void Start();
        public void Tick(float deltaTime);
        public void FixedTick(fix deltaTime);
    }
}
