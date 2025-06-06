using System;

namespace DVG.Core
{
    public readonly struct Command<C> : ICommand, IComparable<Command<C>>
        where C : ICommandData
    {
        public int EntityId { get; }
        public int ClientId { get; }
        public int Tick { get; }
        public C Data { get; }

        public Command(int entityId, int callerId, int tick, C data)
        {
            EntityId = entityId;
            ClientId = callerId;
            Tick = tick;
            Data = data;
        }

        public Command(int callerId, int tick, C data)
        {
            EntityId = 0;
            ClientId = callerId;
            Tick = tick;
            Data = data;
        }

        public readonly int CommandId => Data.CommandId;
        readonly int IComparable<Command<C>>.CompareTo(Command<C> other) => Tick.CompareTo(other);
        readonly int IComparable<ICommand>.CompareTo(ICommand other) => Tick.CompareTo(other.Tick);

        public readonly Command<C> WithEntityId(int entityId)
        {
            return new Command<C>(entityId, ClientId, Tick, Data);
        }

        public readonly Command<C> WithClientId(int callerId)
        {
            return new Command<C>(EntityId, callerId, Tick, Data);
        }
    }
}