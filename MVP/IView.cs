namespace DVG.Core
{
    public interface IView { }
    public interface IView<VM> : IView
        where VM : IViewModel
    {
        void Inject(VM viewModel);
    }
}