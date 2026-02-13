using DVG.Components;
using System.Runtime.Serialization;

namespace DVG.Commands
{
    public struct Command<D> where D : ICommandData
    {
        public ClientId ClientId;
        public int Tick;
        public D Data;

        [IgnoreDataMember]
        public readonly int CommandId => Data.CommandId;

        public Command(ClientId clientId, int tick, D data)
        {
            ClientId = clientId;
            Tick = tick;
            Data = data;
        }

        public readonly Command<D> WithClientId(ClientId clientId) => new(clientId, Tick, Data);
        public readonly Command<D> WithTick(int tick) => new(ClientId, tick, Data);
    }
}