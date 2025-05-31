using System;
using System.Collections.Generic;
using System.Linq;

namespace DVG.Core.Tools
{
    public interface IStringIdProvider
    {
        public Type IdType { get; }
        public IEnumerable<IStringId> Ids { get; }
    }

    public interface IStringIdProvider<Id> : IStringIdProvider
        where Id : IStringId
    {
        public IEnumerable<Id> TypedIds { get; }
        Type IStringIdProvider.IdType => typeof(Id);
        IEnumerable<IStringId> IStringIdProvider.Ids => TypedIds.OfType<IStringId>();
    }
}
