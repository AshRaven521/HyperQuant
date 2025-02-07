using HyperQuantConnector.CoinLayer;
using HyperQuantConnector.Exceptions;
using HyperQuantConnector.Models;
using HyperQuantUI.Core;
using HyperQuantUI.Services;
using System.Collections.ObjectModel;

namespace HyperQuantUI.ViewModel.Wallet
{
    public class WalletViewModel : BaseViewModel
    {
        private readonly ICoinLayerApi coinLayerApi;
        private readonly IDialogService dialogService;

        public WalletViewModel(ICoinLayerApi coinLayerApi, IDialogService dialogService)
        {
            this.coinLayerApi = coinLayerApi;
            this.dialogService = dialogService;
        }

        private ObservableCollection<CryptoBriefCase> briefCases = new ObservableCollection<CryptoBriefCase>();
        public ObservableCollection<CryptoBriefCase> BriefCases
        {
            get
            {
                return briefCases;
            }
            set
            {
                if (briefCases == value)
                {
                    return;
                }
                briefCases = value;
                OnPropertyChanged(nameof(BriefCases));
            }
        }

        private Command getBriefCases;

        public Command GetBriefCases
        {
            get
            {
                if (getBriefCases == null)
                {
                    getBriefCases = new Command(async () => await ShowCryptoCurrencies());
                }
                return getBriefCases;
            }
        }

        private string selectedConfigPath;
        public string SelectedConfigPath
        {
            get
            {
                return selectedConfigPath;
            }
            set
            {
                if (selectedConfigPath == value)
                {
                    return;
                }
                selectedConfigPath = value;
                OnPropertyChanged(nameof(SelectedConfigPath));
            }
        }

        private Command selectConfigFile;
        public Command SelectConfigFile
        {
            get
            {
                if (selectConfigFile == null)
                {
                    selectConfigFile = new Command(SelectFile);
                }
                return selectConfigFile;
            }
        }

        public void SelectFile()
        {
            selectedConfigPath = dialogService.OpenFile();
        }


        public async Task ShowCryptoCurrencies()
        {

            if (string.IsNullOrWhiteSpace(selectedConfigPath))
            {
                dialogService.ShowWarningMessage("Please select configuration file with api key.");
                return;
            }

            var currencies = CreateCurrencies();


            foreach (var currency in currencies)
            {
                IDictionary<string, decimal> exchangeRatesForCurrency = new Dictionary<string, decimal>();
                try
                {
                    // Передаю валюты, затем список валют, кроме уже переданной (список формируется из строкового представления типа валюты)
                    exchangeRatesForCurrency = await coinLayerApi.GetExchangeRates(currency.Type.ToString(), currencies.Select(t => t.Type.ToString()).Where(elem => elem != currency.Type.ToString()), selectedConfigPath);

                }
                catch (CoinLayerApiException e)
                {
                    dialogService.ShowErrorMessage(e.Message);
                    return;
                }

                //Сохраняем список валют в объекте этой валюты (проверяя совпадение ключа словаря(название валюты) с названием валюты)
                if (exchangeRatesForCurrency.Keys.Any(key => key == currency.Type.ToString()))
                {
                    currency.ExchangeRates = exchangeRatesForCurrency;
                }
            }

            var briefs = coinLayerApi.CalculateCurrencyCount(currencies);

            CryptoBriefCase briefCase = new CryptoBriefCase(briefs["USDT"], briefs["BTC"], briefs["XRP"], briefs["XMR"], briefs["DASH"]);

            BriefCases.Add(briefCase);
        }

        private IEnumerable<CryptoCurrency> CreateCurrencies()
        {
            int baseUSDTCount = 0;
            var usdt = new CryptoCurrency
            {
                Type = CryptoType.USDT,
                Count = baseUSDTCount
            };

            int baseBTCCount = 1;
            var btc = new CryptoCurrency
            {
                Type = CryptoType.BTC,
                Count = baseBTCCount
            };

            int baseXRPCount = 15000;
            var xrp = new CryptoCurrency
            {
                Type = CryptoType.XRP,
                Count = baseXRPCount
            };

            int baseXMRCount = 50;
            var xmr = new CryptoCurrency
            {
                Type = CryptoType.XMR,
                Count = baseXMRCount
            };

            int baseDASHCount = 30;
            var dash = new CryptoCurrency
            {
                Type = CryptoType.DASH,
                Count = baseDASHCount
            };

            var currencies = new List<CryptoCurrency>
            {
                usdt,
                btc,
                xrp,
                xmr,
                dash
            };

            return currencies;
        }
    }
}
