using System;

namespace DVG.Core
{
    public interface ICommand: IComparable<ICommand>
    {
        public int EntityId { get; } // reciever id
        public int CommandId { get; } // argument type id
        public int ClientId { get; } // who
        public int Tick { get; } // when
    }
}