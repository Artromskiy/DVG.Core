using System;

namespace DVG.Core
{
    public interface ICommand: IComparable<ICommand>
    {
        public int CommandId { get; } // what
        public int CallerId { get; } // who
        public TimeSpan TimeStamp { get; } // when
    }
}