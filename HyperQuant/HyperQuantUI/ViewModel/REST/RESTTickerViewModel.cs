using HyperQuantConnector.Models;
using HyperQuantConnector.REST;
using HyperQuantUI.Core;
using HyperQuantUI.Services;
using System.Collections.ObjectModel;

namespace HyperQuantUI.ViewModel.REST
{
    public class RESTTickerViewModel : BaseViewModel
    {
        private readonly IRESTClient restClient;
        private readonly IDialogService dialogService;

        public RESTTickerViewModel(IRESTClient restClient,
                                   IDialogService dialogService)
        {
            this.restClient = restClient;
            this.dialogService = dialogService;
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

        private ObservableCollection<Ticker> tickers = new ObservableCollection<Ticker>();

        public ObservableCollection<Ticker> Tickers
        {
            get
            {
                return tickers;
            }
            set
            {
                if (tickers == value)
                {
                    return;
                }
                tickers = value;
                OnPropertyChanged(nameof(Tickers));
            }
        }

        private Command getTickersCommand;
        public Command GetTickersCommand
        {
            get
            {
                if (getTickersCommand == null)
                {
                    getTickersCommand = new Command(async () => await ShowTickers());
                }
                return getTickersCommand;
            }
        }


        public async Task ShowTickers()
        {
            if (string.IsNullOrWhiteSpace(pairValue))
            {
                dialogService.ShowWarningMessage("Fill text boxes with currency pair and maximum trade count!");
                return;
            }

            var tickers = await restClient.GetTickersAsync(pairValue);

            Tickers = new ObservableCollection<Ticker>(tickers);
        }
    }
}
