﻿using HyperQuantUI.Core;
using HyperQuantUI.Services;
using HyperQuantUI.ViewModel.REST;
using HyperQuantUI.ViewModel.Wallet;
using HyperQuantUI.ViewModel.WebSocket;

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

        private Command navigateToWebSocketTradeCommand;
        public Command NavigateToWebSocketTradeCommand
        {
            get
            {
                if (navigateToWebSocketTradeCommand == null)
                {
                    navigateToWebSocketTradeCommand = new Command(Navigation.NavigateTo<WebSocketTradeViewModel>);
                }

                return navigateToWebSocketTradeCommand;
            }
        }

        private Command navigateToWebSocketCandleCommand;
        public Command NavigateToWebSocketCandleCommand
        {
            get
            {
                if (navigateToWebSocketCandleCommand == null)
                {
                    navigateToWebSocketCandleCommand = new Command(Navigation.NavigateTo<WebSocketCandleViewModel>);
                }
                return navigateToWebSocketCandleCommand;
            }
        }

        private Command navigateToBriefCase;
        public Command NavigateToBriefCase
        {
            get
            {
                if (navigateToBriefCase == null)
                {
                    navigateToBriefCase = new Command(Navigation.NavigateTo<WalletViewModel>);
                }
                return navigateToBriefCase;
            }
        }

        private Command navigateToRESTTickerCommand;
        public Command NavigateToRESTTickerCommand
        {
            get
            {
                if (navigateToRESTTickerCommand == null)
                {
                    navigateToRESTTickerCommand = new Command(Navigation.NavigateTo<RESTTickerViewModel>);
                }
                return navigateToRESTTickerCommand;
            }
        }

    }
}
