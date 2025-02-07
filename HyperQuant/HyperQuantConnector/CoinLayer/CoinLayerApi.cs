using HyperQuantConnector.Exceptions;
using HyperQuantConnector.Heplers;
using HyperQuantConnector.Models;
using RestSharp;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text.Json.Nodes;

namespace HyperQuantConnector.CoinLayer
{
    public class CoinLayerApi : ICoinLayerApi
    {
        /// <summary>
        /// Подсчет валютного портфеля
        /// </summary>
        /// <param name="currencies"></param>
        /// <returns></returns>
        public IDictionary<string, decimal> CalculateCurrencyCount(IEnumerable<CryptoCurrency> currencies)
        {

            IDictionary<string, decimal> baseCryptoBriefCase = new Dictionary<string, decimal>
            {
                {"USDT",  0},
                {"BTC", 1},
                {"XRP", 15000 },
                {"XMR", 50},
                {"DASH", 30}
            };

            IDictionary<string, decimal> calculated = new Dictionary<string, decimal>();

            //Перебираем все валюты
            foreach (var currency in currencies)
            {
                decimal currencyCount = 0m;

                // Переводим весь базовый портфель валют в одну валюту
                foreach (var pair in currency.ExchangeRates)
                {
                    currencyCount += pair.Value * baseCryptoBriefCase[pair.Key];
                }

                calculated.Add(currency.Type.ToString(), currencyCount);
            }
            return calculated;
        }

        public async Task<IDictionary<string, decimal>> GetExchangeRates(string currency, IEnumerable<string> neededCurrencies, string configFilePath)
        {

            string apiKey = ConfigurationHelper.GetApiKey(configFilePath);

            string symbols = currency + "," + string.Join(",", neededCurrencies);

            var options = new RestClientOptions($"https://api.coinlayer.com/live?access_key={apiKey}&target={currency}&symbols={symbols}");
            var client = new RestClient(options);
            var request = new RestRequest("");

            request.AddHeader("accept", "application/json");

            var response = await client.GetAsync(request);

            // Проверяю, чтобы запрос выполнился успешно и у ответ был не пустой и не содержал ошибок
            if (response.IsSuccessful && !string.IsNullOrWhiteSpace(response.Content) && !response.Content.Contains("error"))
            {
                IDictionary<string, decimal> rates = new Dictionary<string, decimal>();

                var obj = JsonObject.Parse(response.Content);

                if (obj["rates"] is JsonObject jsonRates)
                {
                    foreach (var rate in jsonRates)
                    {
                        /* Пояснение! Coin Layer API на запрос о курсе валюты возвращает массив [валюта, сколько стоит эта валюта относительно запрашиваемой] */
                        decimal currencyRate = decimal.Parse(rate.Value?.ToString(), NumberStyles.AllowExponent | NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture);

                        rates.Add(rate.Key, currencyRate);
                    }
                }

                return rates;

            }
            else
            {
                var obj = JsonObject.Parse(response.Content);
                if (obj["error"] is JsonObject error)
                {
                    int errorCode = int.Parse(error["code"].ToString());
                    string errorMessage = error["info"].ToString();
                    string resultMessage = $"There is an error from Coin Layer API with code {errorCode}. Message: {errorMessage}";
                    throw new CoinLayerApiException(resultMessage);
                }
                throw new CoinLayerApiException("There is an error while parsing response from Coin Layer API");
            }
        }
    }
}
