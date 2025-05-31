namespace DVG.Core
{
    public interface IFactory { }

    public interface IFactory<T> : IFactory
    {
        public T Create();
        public void Dispose(T instance);
    }
    public interface IFactory<T, P> : IFactory
    {
        public T Create(P parameters);
        public void Dispose(T instance);
    }

}
