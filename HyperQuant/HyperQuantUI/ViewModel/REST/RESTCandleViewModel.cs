using HyperQuantConnector.Heplers;
using HyperQuantConnector.Models;
using HyperQuantConnector.REST;
using HyperQuantUI.Core;
using HyperQuantUI.Services;
using System.Collections.ObjectModel;

namespace HyperQuantUI.ViewModel.REST
{
    public class RESTCandleViewModel : BaseViewModel
    {
        private readonly IRESTClient restClient;
        private readonly IDialogService dialogService;

        public RESTCandleViewModel(IRESTClient restClient,
                             IDialogService dialogService)
        {
            this.restClient = restClient;
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

        private DateTime selectedStartDate;
        public DateTime SelectedStartDate
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
            if (string.IsNullOrWhiteSpace(pairValue) || string.IsNullOrWhiteSpace(maxCandlesCountValue))
            {
                dialogService.ShowWarningMessage("Fill text boxes with currency pair and maximum candles count!");
                return;
            }

            int period = CustomConverter.GetCandlePeriodByQueryParam(selectedQueryParam);

            DateTimeOffset startDateOffset = selectedStartDate;

            DateTimeOffset? endDateOffset = selectedEndDate;

            int maxCount = 0;
            if (int.TryParse(maxCandlesCountValue, out int temp))
            {
                maxCount = temp;
            }
            else
            {
                dialogService.ShowWarningMessage("Entered maximum trade count is not a number!");
                return;
            }


            var downloadedCandles = await restClient.GetCandleSeriesAsync(pairValue, period, startDateOffset, endDateOffset, maxCount);
            Candles = new ObservableCollection<Candle>(downloadedCandles);
        }
    }
}
