namespace DVG.Core
{
    public interface IFactory { }

    public interface IFactory<T> : IFactory
    {
        T Create();
    }
    public interface IFactory<T, P> : IFactory
    {
        T Create(P parameters);
    }

}
