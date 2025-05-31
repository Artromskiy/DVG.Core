using System.Collections.Generic;

namespace DVG.Core.Tools
{
    public abstract class StringIdProvider<T, V> : IStringIdProvider<V>
        where T : StringIdProvider<T, V>, new()
        where V : IStringId

    {
        public static T Instance => new();
        public abstract IEnumerable<V> TypedIds { get; }
    }
}
