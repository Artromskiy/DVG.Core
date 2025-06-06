namespace DVG.Core
{
    public interface IGenericAction<K>
    {
        void Invoke<T>() where T : K;
    }

    public interface IGenericAction
    {
        void Invoke<T>();
    }
}
