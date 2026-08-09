namespace KibiHex.Ids
{
	public interface IId
	{
		string Value { get; }
		bool IsNone { get; }
	}
}
