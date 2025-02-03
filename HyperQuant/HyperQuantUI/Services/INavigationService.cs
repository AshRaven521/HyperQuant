using HyperQuantUI.ViewModel;

namespace HyperQuantUI.Services
{
    public interface INavigationService
    {
        BaseViewModel CurrentView { get; }
        void NavigateTo<T>() where T : BaseViewModel;
    }
}
