using KibiHex.Components.Attributes;
using KibiHex.NewType;

namespace KibiHex.Components
{
	[Component(true)]
	public struct SyncId : INewType<int>
	{
		public int Value;

		int INewType<int>.Value { readonly get => Value; set => Value = value; }

		public static implicit operator SyncId(int value) => new() { Value = value };

		public static implicit operator int(SyncId newType) => newType.Value;
	}
}
