using HyperQuantConnector.Models;
using HyperQuantUI.Core;
using HyperQuantUI.Services;
using System.Collections.ObjectModel;
using HyperQuantConnector.WebSocket;
using HyperQuantConnector.Heplers;

namespace HyperQuantUI.ViewModel.WebSocket
{
    public class WebSocketCandleViewModel : BaseViewModel
    {
        private readonly IWebSocketClient webSocketClient;
        private readonly IDialogService dialogService;

        public WebSocketCandleViewModel(IWebSocketClient webSocketClient,
                             IDialogService dialogService)
        {
            this.webSocketClient = webSocketClient;
            this.dialogService = dialogService;
        }

        private Command getCandlesCommand;
        public Command GetCandlesCommand
        {
            get
            {
                if (getCandlesCommand == null)
                {
                    getCandlesCommand = new Command(async () => await ShowCandles());
                }
                return getCandlesCommand;
            }
        }

        private ObservableCollection<Candle> candles = new ObservableCollection<Candle>();

        public ObservableCollection<Candle> Candles
        {
            get
            {
                return candles;
            }
            set
            {
                if (candles == value)
                {
                    return;
                }
                candles = value;
                OnPropertyChanged(nameof(Candles));
            }
        }

        private string selectedQueryParam;

        public string SelectedQueryParam
        {
            get
            {
                return selectedQueryParam;
            }
            set
            {
                if (selectedQueryParam == value)
                {
                    return;
                }
                selectedQueryParam = value;
                OnPropertyChanged(nameof(SelectedQueryParam));
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

        private string maxCandlesCountValue;

        public string MaxCandlesCountValue
        {
            get
            {
                return maxCandlesCountValue;
            }
            set
            {
                if (maxCandlesCountValue == value)
                {
                    return;
                }
                maxCandlesCountValue = value;
                OnPropertyChanged(nameof(MaxCandlesCountValue));
            }
        }

        private DateTime? selectedStartDate;
        public DateTime? SelectedStartDate
        {
            get
            {
                return selectedStartDate;
            }
            set
            {
                if (selectedStartDate == value)
                {
                    return;
                }
                selectedStartDate = value;
                OnPropertyChanged(nameof(SelectedStartDate));
            }
        }

        private DateTime? selectedEndDate;
        public DateTime? SelectedEndDate
        {
            get
            {
                return selectedEndDate;
            }
            set
            {
                if (selectedEndDate == value)
                {
                    return;
                }
                selectedEndDate = value;
                OnPropertyChanged(nameof(SelectedEndDate));
            }
        }

        private ObservableCollection<string> queryParams = new ObservableCollection<string>(ConnectorConstants.QueryPeriodInSeconds.Keys);

        public ObservableCollection<string> QueryParams
        {
            get
            {
                return queryParams;
            }
            set
            {
                if (queryParams == value)
                {
                    return;
                }
                queryParams = value;
                OnPropertyChanged(nameof(QueryParams));
            }
        }


        public async Task ShowCandles()
        {
            if (string.IsNullOrWhiteSpace(pairValue))
            {
                dialogService.ShowWarningMessage("Fill text box with currency pair !");
                return;
            }

            int period = CustomConverter.GetCandlePeriodByQueryParam(selectedQueryParam);

            DateTimeOffset? startDateOffset = selectedStartDate;

            DateTimeOffset? endDateOffset = selectedEndDate;

            long? maxCount = null;

            if (!string.IsNullOrWhiteSpace(maxCandlesCountValue) && !long.TryParse(maxCandlesCountValue, out _))
            {
                dialogService.ShowWarningMessage("Entered maximum trade count is not a number!");
                return;
            }
            else if(!string.IsNullOrWhiteSpace(maxCandlesCountValue) && long.TryParse(maxCandlesCountValue, out long count))
            {
                maxCount = count;
            }

            await webSocketClient.SubscribeCandles(pairValue, period, startDateOffset, endDateOffset, maxCount);


            //var downloadedCandles = await restClient.GetCandleSeriesAsync(pairValue, period, startDateOffset, endDateOffset, maxCount);
            //Candles = new ObservableCollection<Candle>(downloadedCandles);
        }
    }
}
