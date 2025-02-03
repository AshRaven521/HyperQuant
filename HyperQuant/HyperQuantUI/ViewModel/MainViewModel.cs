using HyperQuantUI.Core;
using HyperQuantUI.Services;
using HyperQuantUI.ViewModel.REST;

namespace HyperQuantUI.ViewModel
{
    public class MainViewModel : BaseViewModel
    {
        private INavigationService navigation;
        public INavigationService Navigation
        {
            get
            {
                return navigation;
            }
            set
            {
                if (navigation == value)
                {
                    return;
                }
                navigation = value;
                OnPropertyChanged(nameof(Navigation));
            }
        }
        public MainViewModel(INavigationService navigationService)
        {
            Navigation = navigationService;
        }

        private Command navigateToRESTTradeCommand;
        public Command NavigateToRESTTradeCommand
        {
            get
            {
                if (navigateToRESTTradeCommand == null)
                {
                    navigateToRESTTradeCommand = new Command(Navigation.NavigateTo<RESTTradeViewModel>);
                }

                return navigateToRESTTradeCommand;
            }
        }

        private Command navigateToRESTCandleCommand;
        public Command NavigateToRESTCandleCommand
        {
            get
            {
                if (navigateToRESTCandleCommand == null)
                {
                    navigateToRESTCandleCommand = new Command(Navigation.NavigateTo<RESTCandleViewModel>);
                }

                return navigateToRESTCandleCommand;
            }
        }

    }
}
