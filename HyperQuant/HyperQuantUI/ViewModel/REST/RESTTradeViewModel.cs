using HyperQuantConnector.Models;
using HyperQuantConnector.REST;
using HyperQuantUI.Core;
using HyperQuantUI.Services;
using System.Collections.ObjectModel;

namespace HyperQuantUI.ViewModel.REST
{
    public class RESTTradeViewModel : BaseViewModel
    {
        private readonly IRESTClient restClient;
        private readonly IDialogService dialogService;
        public RESTTradeViewModel(IRESTClient restClient,
                             IDialogService dialogService)
        {
            this.restClient = restClient;
            this.dialogService = dialogService;
        }

        private Command getTradesCommand;
        public Command GetTradesCommand
        {
            get
            {
                if (getTradesCommand == null)
                {
                    getTradesCommand = new Command(async () => await ShowTrades());
                }
                return getTradesCommand;
            }
        }

        private ObservableCollection<Trade> trades = new ObservableCollection<Trade>();

        public ObservableCollection<Trade> Trades
        {
            get
            {
                return trades;
            }
            set
            {
                if (trades == value)
                {
                    return;
                }
                trades = value;
                OnPropertyChanged(nameof(Trades));
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

        public async Task ShowTrades()
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

            var downloadedTrades = await restClient.GetNewTradesAsync(pairValue, maxCount);

            if (!downloadedTrades.Any())
            {
                dialogService.ShowErrorMessage("BitnefixAPI return empty array!\nPlease check entered currency value. Problem can be here.");
            }

            Trades = new ObservableCollection<Trade>(downloadedTrades);
        }
    }
}
