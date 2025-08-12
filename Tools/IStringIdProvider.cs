using System;
using System.Collections.Generic;
using System.Linq;

namespace DVG.Core.Tools
{
    public interface IStringIdProvider
    {
        public Type IdType { get; }
        public IEnumerable<IId> Ids { get; }
    }

    public interface IStringIdProvider<Id> : IStringIdProvider
        where Id : IId
    {
        public IEnumerable<Id> TypedIds { get; }
        Type IStringIdProvider.IdType => typeof(Id);
        IEnumerable<IId> IStringIdProvider.Ids => TypedIds.OfType<IId>();
    }
}
