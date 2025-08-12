namespace DVG.Core
{
    public interface IView { }
    public interface IView<VM>
        where VM : IViewModel
    {
        [Inject]
        VM ViewModel { get; set; }
    }
}