namespace DVG.Core
{
    public interface IMementoable<T>
        where T: IMementoData
    {
        T GetMemento();
        void SetMemento(T memento);
    }
}
