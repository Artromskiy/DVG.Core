namespace DVG
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

    public interface IStructGenericAction<K>
    {
        void Invoke<T>() where T : struct, K;
    }

    public interface IGenericCaller
    {
        void Call<T>(ref T action) where T : IGenericAction;
    }

    public interface IGenericCaller<K>
    {
        void ForEach<T>(ref T action) where T : IGenericAction<K>;
    }

    public interface IStructGenericCaller
    {
        void ForEach<T>(ref T action) where T : IStructGenericAction;
    }

    public interface IStructGenericCaller<K>
    {
        void ForEach<T>(ref T action) where T : IStructGenericAction<K>;
    }
}