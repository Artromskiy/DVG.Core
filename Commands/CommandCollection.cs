using DVG.Core;
using System;
using System.Collections;
using System.Collections.Generic;

namespace DVG.Commands
{
    public sealed class CommandCollection
    {
        private readonly Dictionary<Type, IDictionary> _lists = new();

        public void Add<T>(Command<T> value)
            where T : ICommandData
        {
            var key = typeof(T);

            if (!_lists.TryGetValue(key, out var list))
                _lists.Add(key, list = new ClientCommands<T>());

            if (list is not ClientCommands<T> generic)
                throw new InvalidOperationException();
            try
            {
                generic.Add(value.ClientId, value);
            }
            catch (Exception e)
            {
                throw new InvalidOperationException
                    ($"Attempt to add command of type {key.Name} for client {value.ClientId} at {value.Tick}", e);
            }
        }

        public bool Remove<T>(int clientId)
            where T : ICommandData
        {
            var key = typeof(T);

            if (!_lists.TryGetValue(key, out var list))
                return false;

            if (list is not ClientCommands<T> generic)
                throw new InvalidOperationException();

            return generic.Remove(clientId);
        }

        public void Clear() => _lists.Clear();

        public void Clear<T>()
            where T : ICommandData
        {
            var key = typeof(T);
            if (!_lists.TryGetValue(key, out var list))
                return;

            if (list is not ClientCommands<T> generic)
                throw new InvalidOperationException();

            generic.Clear();
        }

        public IReadOnlyCollection<Command<T>>? GetCollection<T>()
            where T : ICommandData
        {
            var key = typeof(T);
            if (!_lists.TryGetValue(key, out var list))
                return null;

            if (list is not ClientCommands<T> generic)
                throw new InvalidOperationException();

            return generic.Values;
        }

        private sealed class ClientCommands<T> : SortedDictionary<int, Command<T>>
            where T : ICommandData
        { }
    }
}
