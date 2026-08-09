using KibiHex.Components.Attributes;
using KibiHex.NewType;

namespace KibiHex.Components
{
	[Component(true)]
	public struct ClientId : INewType<int>
	{
		public int Value;

		int INewType<int>.Value { readonly get => Value; set => Value = value; }

		public static implicit operator ClientId(int value) => new() { Value = value };

		public static implicit operator int(ClientId newType) => newType.Value;
	}
}
