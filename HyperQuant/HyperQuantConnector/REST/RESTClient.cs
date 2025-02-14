﻿using HyperQuantConnector.Heplers;
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
            // Если приходит funding currency, то получаем все, без аггрегации, т.е. (а30:p2:p30)
            if (pair.StartsWith('f'))
            {
                if (to == null)
                {
                    uriBuilder.AppendLine($"https://api-pub.bitfinex.com/v2/candles/trade%3A{periodParam}%3A{pair}%3Aa30%3Ap2%3Ap30/hist?sort=-1&start={from}&limit={count}");
                }
                else
                {
                    uriBuilder.AppendLine($"https://api-pub.bitfinex.com/v2/candles/trade%3A{periodParam}%3A{pair}%3Aa30%3Ap2%3Ap30/hist?sort=-1&start={from}&end={to}&limit={count}");
                }
            }
            else
            {
                if (to == null)
                {
                    uriBuilder.AppendLine($"https://api-pub.bitfinex.com/v2/candles/trade%3A{periodParam}%3A{pair}/hist?sort=-1&start={from}&limit={count}");
                }
                else
                {
                    uriBuilder.AppendLine($"https://api-pub.bitfinex.com/v2/candles/trade%3A{periodParam}%3A{pair}/hist?sort=-1&start={from}&end={to}&limit={count}");
                }
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
                if (pair.StartsWith('f'))
                {
                    var fundingTrades = CustomConverter.ParseTrades(response.Content, true).ToList();
                    return fundingTrades;
                }
                var trades = CustomConverter.ParseTrades(response.Content).ToList();

                return trades;
            }

            return new List<Trade>();

        }

        public async Task<IEnumerable<Ticker>> GetTickersAsync(string pair)
        {
            var options = new RestClientOptions($"https://api-pub.bitfinex.com/v2/ticker/{pair}");
            var client = new RestClient(options);
            var request = new RestRequest("");

            request.AddHeader("accept", "application/json");

            var response = await client.GetAsync(request);

            if (response.IsSuccessful && !string.IsNullOrWhiteSpace(response.Content))
            {
                if(pair.StartsWith('f'))
                {
                    var fundingTickers = CustomConverter.ParseTickers(response.Content, true).ToList();
                    return fundingTickers;
                }
                var tickers = CustomConverter.ParseTickers(response.Content).ToList();

                return tickers;
            }

            return new List<Ticker>();
        }
    }
}
