namespace DVG.Core
{
    public interface IView { }
    public interface IView<VM>
        where VM : IViewModel
    {
        void Inject(VM viewModel);
    }
}