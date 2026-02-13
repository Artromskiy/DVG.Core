using DVG.Components.Attributes;
using DVG.NewType;

namespace DVG.Components
{
    [Component(true)]
    public struct SyncId : INewType<int>
    {
        public int Value;

        int INewType<int>.Value { readonly get => Value; set => Value = value; }

        public static implicit operator SyncId(int value)
            => new() { Value = value };

        public static implicit operator int(SyncId newType)
            => newType.Value;
    }
}
