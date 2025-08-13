using System;
using System.Collections.Generic;

namespace DVG.Core
{
    public sealed class PlayerLoopSystem : IPlayerLoopSystem
    {
        private readonly HashSet<ITickable> _tickables = new();
        private readonly HashSet<IFixedTickable> _fixedTickables = new();
        private readonly HashSet<IStartable> _startables = new();

        private readonly List<IPlayerLoopItem> _toRemove = new();

        public event Action<Exception>? ExceptionHandler;

        private bool _disposed;

        public void Add(IPlayerLoopItem item)
        {
            if (_disposed)
                return;

            TryAddToSet(_startables, item);
            TryAddToSet(_tickables, item);
            TryAddToSet(_fixedTickables, item);
        }

        public void Remove(IPlayerLoopItem item)
        {
            if (_disposed)
                return;

            _toRemove.Add(item);
        }

        public void Start()
        {
            foreach (var item in _startables)
            {
                try
                {
                    item.Start();
                }
                catch (Exception e)
                {
                    ExceptionHandler?.Invoke(e);
                }
            }
            _startables.Clear();
        }

        public void Tick(float deltaTime)
        {
            foreach (var item in _tickables)
            {
                try
                {
                    item.Tick(deltaTime);
                }
                catch (Exception e)
                {
                    ExceptionHandler?.Invoke(e);
                }
            }
        }

        public void FixedTick(fix deltaTime)
        {
            foreach (var item in _fixedTickables)
            {
                try
                {
                    item.Tick(deltaTime);
                }
                catch (Exception e)
                {
                    ExceptionHandler?.Invoke(e);
                }
            }
        }

        public void EndOfFrame()
        {
            foreach (var item in _toRemove)
            {
                if (item is IStartable startable)
                    _startables.Remove(startable);
                TryToRemoveFromSet(_tickables, item);
                TryToRemoveFromSet(_fixedTickables, item);
            }
            _toRemove.Clear();
        }

        private void TryAddToSet<T>(HashSet<T> set, IPlayerLoopItem item) where T : IPlayerLoopItem
        {
            if (item is not T castedItem)
                return;
            if (set.Add(castedItem))
                return;
            ExceptionHandler?.Invoke(new InvalidOperationException($"object of type {item.GetType().FullName} added to loop multiple times"));
        }

        private void TryToRemoveFromSet<T>(HashSet<T> set, IPlayerLoopItem item) where T : IPlayerLoopItem
        {
            if (item is not T castedItem)
                return;
            if (set.Remove(castedItem))
                return;
            ExceptionHandler?.Invoke(new InvalidOperationException($"object of type {item.GetType().FullName} is already removed from loop. Probably object is disposed multiple times"));
        }

        public void Dispose()
        {
            _disposed = true;
            _startables.Clear();
            _tickables.Clear();
            _fixedTickables.Clear();
        }
    }
}
