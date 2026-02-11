using DVG.Components.Attributes;

namespace DVG.Components
{
    [Component(true)]
    public struct SyncIdReserve
    {
        public int First;
        public int Count;
        public int Current;
    }
}
