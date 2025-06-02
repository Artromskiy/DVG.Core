namespace DVG.Core
{
    public interface ICommandable<T>
        where T: unmanaged, ICommandData
    {
        void Recieve(T cmd);
    }
}
