using System;
using System.Runtime.Serialization;

namespace DVG.Core
{
    [DataContract]
    public readonly struct Command<D> : ICommand, IComparable<Command<D>>
        where D : ICommandData
    {
        [DataMember(Order = 0)]
        public int EntityId { get; }
        [DataMember(Order = 1)]
        public int ClientId { get; }
        [DataMember(Order = 2)]
        public int Tick { get; }
        [DataMember(Order = 3)]
        public D Data { get; }

        [IgnoreDataMember]
        public readonly int CommandId => Data.CommandId;

        public Command(int entityId, int callerId, int tick, D data)
        {
            EntityId = entityId;
            ClientId = callerId;
            Tick = tick;
            Data = data;
        }

        public Command(int callerId, int tick, D data)
        {
            EntityId = 0;
            ClientId = callerId;
            Tick = tick;
            Data = data;
        }

        readonly int IComparable<Command<D>>.CompareTo(Command<D> other) => Tick.CompareTo(other);
        readonly int IComparable<ICommand>.CompareTo(ICommand other) => Tick.CompareTo(other.Tick);
        public readonly Command<D> WithEntityId(int entityId) => new Command<D>(entityId, ClientId, Tick, Data);
        public readonly Command<D> WithClientId(int callerId) => new Command<D>(EntityId, callerId, Tick, Data);
    }
}