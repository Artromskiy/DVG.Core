using System;

namespace DVG.Core
{
    public interface ICommand : IComparable<ICommand>
    {
        int EntityId { get; } // reciever id
        int CommandId { get; } // generic argument type id
        int ClientId { get; } // who
        int Tick { get; } // when
    }
}