using System;
using System.Collections;
using System.Collections.Generic;

namespace DVG.Core
{
    public sealed class CommandCollection
    {
        private readonly Dictionary<Type, IDictionary> _lists = new Dictionary<Type, IDictionary>();

        public void Add<T>(Command<T> value)
            where T : ICommandData
        {
            var key = typeof(T);

            if (!_lists.TryGetValue(key, out var list))
                _lists.Add(key, list = new ClientCommands<T>());

            if (list is not ClientCommands<T> generic)
                throw new InvalidOperationException();

            generic.Add(value.ClientId, value);
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

        public void Trim<T>()
            where T : ICommandData
        {
            var key = typeof(T);
            if (!_lists.TryGetValue(key, out var list))
                return;

            if (list is not ClientCommands<T> generic)
                throw new InvalidOperationException();

            //generic.TrimExcess();
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
