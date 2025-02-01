using HyperQuantConnector.Models;
using RestSharp;

namespace HyperQuantConnector.REST
{
    public class RESTClient : IRESTClient
    {
        public Task<IEnumerable<Candle>> GetCandleSeriesAsync(string pair, int periodInSec, DateTimeOffset? from, DateTimeOffset? to = null, long? count = 0)
        {
            throw new NotImplementedException();
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
                var trades = JSONParser.ParseTrades(response.Content).ToList();

                return trades;
            }
            
            return new List<Trade>();

        }
    }
}
