using HyperQuantConnector.Models;
using RestSharp;
using System.Text;

namespace HyperQuantConnector.REST
{
    public class RESTClient : IRESTClient
    {
        public async Task<IEnumerable<Candle>> GetCandleSeriesAsync(string pair, int periodInSec, DateTimeOffset? from, DateTimeOffset? to = null, long? count = 0)
        {
            string periodParam = CustomConverter.GetCandleQueryParamByPeriod(periodInSec);

            StringBuilder uriBuilder = new StringBuilder();

            if (to == null)
            {
                uriBuilder.AppendLine($"https://api-pub.bitfinex.com/v2/candles/trade%3A{periodParam}%3A{pair}/hist?sort=-1&start={from}&limit={count}");
            }
            else
            {
                uriBuilder.AppendLine($"https://api-pub.bitfinex.com/v2/candles/trade%3A{periodParam}%3A{pair}/hist?sort=-1&start={from}&end={to}&limit={count}");
            }

            var options = new RestClientOptions(uriBuilder.ToString());
            var client = new RestClient(options);

            var request = new RestRequest("");
            request.AddHeader("accept", "application/json");
            var response = await client.GetAsync(request);

            if (response.IsSuccessful && !string.IsNullOrWhiteSpace(response.Content))
            {
                var candles = CustomConverter.ParseCandles(response.Content);
                return candles;
            }

            return new List<Candle>();
        }

        public async Task<IEnumerable<Trade>> GetNewTradesAsync(string pair, int maxCount)
        {
            var options = new RestClientOptions($"https://api-pub.bitfinex.com/v2/trades/{pair}/hist?limit={maxCount}&sort=-1");
            var client = new RestClient(options);
            var request = new RestRequest("");

            request.AddHeader("accept", "application/json");

            var response = await client.GetAsync(request);

            if (response.IsSuccessful && !string.IsNullOrWhiteSpace(response.Content))
            {
                var trades = CustomConverter.ParseTrades(response.Content).ToList();

                return trades;
            }
            
            return new List<Trade>();

        }
    }
}
