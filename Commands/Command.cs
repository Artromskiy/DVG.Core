using DVG.Commands;
using System;
using System.Runtime.Serialization;

namespace DVG.Core
{
    [DataContract]
    public struct Command<D> : ICommand, IComparable<Command<D>>
        where D : ICommandData
    {
        [DataMember(Order = 0)]
        public int EntityId { get; set; }
        [DataMember(Order = 1)]
        public int ClientId { get; set; }
        [DataMember(Order = 2)]
        public int Tick { get; set; }
        [DataMember(Order = 3)]
        public D Data { get; set; }

        [IgnoreDataMember]
        public readonly int CommandId => Data.CommandId;

        public Command(int entityId, int callerId, int tick, D data)
        {
            EntityId = entityId;
            ClientId = callerId;
            Tick = tick;
            Data = data;
        }

        readonly int IComparable<Command<D>>.CompareTo(Command<D> other) => Tick.CompareTo(other);
        readonly int IComparable<ICommand>.CompareTo(ICommand other) => Tick.CompareTo(other.Tick);
        public readonly Command<D> WithEntityId(int entityId) => new Command<D>(entityId, ClientId, Tick, Data);
        public readonly Command<D> WithClientId(int clientId) => new Command<D>(EntityId, clientId, Tick, Data);
        public readonly Command<D> WithTick(int tick) => new Command<D>(EntityId, ClientId, tick, Data);
    }
}