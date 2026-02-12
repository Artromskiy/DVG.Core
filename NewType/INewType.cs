namespace DVG.NewType
{
    public interface INewType { }

    public interface INewType<T>
    {
        T Value { get; set; }
    }
}
