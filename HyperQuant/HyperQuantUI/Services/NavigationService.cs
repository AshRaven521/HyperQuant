using HyperQuantUI.ViewModel;

namespace HyperQuantUI.Services
{
    public class NavigationService : BaseViewModel, INavigationService
    {
        private BaseViewModel currentView;
        private readonly Func<Type, BaseViewModel> viewModelFactory;

        public BaseViewModel CurrentView
        {
            get
            {
                return currentView;
            }
            private set
            {
                if (currentView == value)
                {
                    return;
                }
                currentView = value;
                OnPropertyChanged(nameof(CurrentView));
            }

        }

        public NavigationService(Func<Type, BaseViewModel> viewModelFactory)
        {
            this.viewModelFactory = viewModelFactory;
        }

        public void NavigateTo<TVIewModel>() where TVIewModel : BaseViewModel
        {
            BaseViewModel viewModel = viewModelFactory.Invoke(typeof(TVIewModel));
            CurrentView = viewModel;
        }
    }
}
