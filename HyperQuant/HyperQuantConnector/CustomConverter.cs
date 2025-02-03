using HyperQuantConnector.Models;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;

namespace HyperQuantConnector
{
    public class CustomConverter
    {
        /* Парсим вручную ответ с API Bitnefix при запросе данных на пару валют */
        public static IEnumerable<Trade> ParseTrades(string json)
        {
            var dataArray = json.Replace(']', ' ');
            var splittedDataArray = dataArray.Split('[');

            List<Trade> result = new List<Trade>();

            foreach (string splittedData in splittedDataArray)
            {
                if (string.IsNullOrWhiteSpace(splittedData))
                {
                    continue;
                }
                var splittedByComaArray = splittedData.Split(',');



                Trade trade = new Trade
                {
                    Id = splittedByComaArray[0],
                    Time = DateTimeOffset.FromUnixTimeMilliseconds(long.Parse(splittedByComaArray[1])),
                    Pair = splittedByComaArray[2],
                    Price = decimal.Parse(splittedByComaArray[3])
                };

                result.Add(trade);

            }


            return result;
        }

        public static IEnumerable<Candle> ParseCandles(string json)
        {
            var dataArray = json.Replace(']', ' ');
            var splittedDataArray = dataArray.Split('[');

            List<Candle> result = new List<Candle>();

            foreach (string splittedData in splittedDataArray)
            {
                if (string.IsNullOrWhiteSpace(splittedData))
                {
                    continue;
                }
                var splittedByComaArray = splittedData.Split(',');

                IFormatProvider formatter = new NumberFormatInfo { NumberDecimalSeparator = "." };

                Candle candle = new Candle
                {
                    OpenTime = DateTimeOffset.FromUnixTimeMilliseconds(long.Parse(splittedByComaArray[0])),
                    Open = int.Parse(splittedByComaArray[1]),
                    Close = int.Parse(splittedByComaArray[2]),
                    High = int.Parse(splittedByComaArray[3]),
                    Low = int.Parse(splittedByComaArray[4]),
                    Volume = double.Parse(splittedByComaArray[5], formatter)
                };

                result.Add(candle);

            }


            return result;
        }

        public static string GetCandleQueryParamByPeriod(int period)
        {
            foreach (var pair in ConnectorConstants.QueryPeriodInSeconds)
            {
                if (pair.Value == period)
                {
                    return pair.Key;
                }
            }

            return ConnectorConstants.DEFAULT_PERIOD_QUERY_PARAM;
        }

        public static int GetCandlePeriodByQueryParam(string param)
        {
            return ConnectorConstants.QueryPeriodInSeconds[param];
        }

       
    }
}
