using KibiHex.Components.Attributes;

namespace KibiHex.Components
{
	[Component(true)]
	public struct SyncIdReserve
	{
		public int First;
		public int Count;
		public int Current;
	}
}
