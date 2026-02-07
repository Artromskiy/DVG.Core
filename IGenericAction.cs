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

    public interface IStructGenericAction
    {
        void Invoke<T>() where T : struct;
    }

    public interface IStructGenericCaller
    {
        void ForEach<T>(ref T action) where T : IStructGenericAction;
    }
}