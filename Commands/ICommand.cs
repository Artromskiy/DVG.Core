using DVG.Components;

namespace DVG.Commands
{
    public interface ICommand
    {
        ClientId ClientId { get; }
        int CommandId { get; }
        int Tick { get; }
    }
}