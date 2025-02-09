using HyperQuantConnector.Models;
using HyperQuantConnector.WebSocket;
using HyperQuantUI.Core;
using HyperQuantUI.Services;
using System.Collections.ObjectModel;

namespace HyperQuantUI.ViewModel.WebSocket
{
    public class WebSocketTradeViewModel : BaseViewModel
    {
        private readonly IWebSocketClient webSocketClient;
        private readonly IDialogService dialogService;

        public WebSocketTradeViewModel(IWebSocketClient webSocketClient, IDialogService dialogService)
        {
            this.webSocketClient = webSocketClient;
            this.dialogService = dialogService;
        }

        private Command getWebSocketTradesCommand;

        public Command GetWebSocketTradesCommand
        {
            get
            {
                if (getWebSocketTradesCommand == null)
                {
                    getWebSocketTradesCommand = new Command(async () => await ShowWebSocketTrades());
                }
                return getWebSocketTradesCommand;
            }
        }

        private ObservableCollection<Trade> webSocketTrades = new ObservableCollection<Trade>();
        public ObservableCollection<Trade> WebSocketTrades
        {
            get
            {
                return webSocketTrades;
            }
            set
            {
                if (webSocketTrades == value)
                {
                    return;
                }
                webSocketTrades = value;
                OnPropertyChanged(nameof(WebSocketTrades));
            }
        }

        private string pairValue;
        public string PairValue
        {
            get
            {
                return pairValue;
            }
            set
            {
                if (pairValue == value)
                {
                    return;
                }
                pairValue = value;
                OnPropertyChanged(nameof(PairValue));
            }
        }

        private string maxTradesCountValue;

        public string MaxTradesCountValue
        {
            get
            {
                return maxTradesCountValue;
            }
            set
            {
                if (maxTradesCountValue == value)
                {
                    return;
                }
                maxTradesCountValue = value;
                OnPropertyChanged(nameof(MaxTradesCountValue));
            }
        }

        public async Task ShowWebSocketTrades()
        {
            if (string.IsNullOrWhiteSpace(pairValue) || string.IsNullOrWhiteSpace(maxTradesCountValue))
            {
                dialogService.ShowWarningMessage("Fill text boxes with currency pair and maximum trade count!");
                return;
            }

            int maxCount = 0;
            if (int.TryParse(maxTradesCountValue, out int temp))
            {
                maxCount = temp;
            }
            else
            {
                dialogService.ShowWarningMessage("Entered maximum trade count is not a number!");
            }
            await webSocketClient.SubscribeTrades(pairValue, maxCount);
            WebSocketTrades = new ObservableCollection<Trade>(webSocketClient.ParsedTrades);
        }
    }
}
