namespace DVG.Core
{
    public interface ICommandable<T>
        where T: ICommandData
    {
        void Execute(T cmd);
    }
}
