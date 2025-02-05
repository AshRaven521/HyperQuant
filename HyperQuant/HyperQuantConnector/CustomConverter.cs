using HyperQuantConnector.Models;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;

namespace HyperQuantConnector
{
    public class CustomConverter
    {
        /* Парсим вручную ответ с API Bitnefix при запросе данных на пару валют */
        public static IEnumerable<Trade> ParseTrades(string json, bool isFundingCurrency = false)
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

                IFormatProvider formatter = new NumberFormatInfo { NumberDecimalSeparator = "." };

                Trade trade = new Trade();
                // Если рассматриваем пару валют (например, tBTCUSD). Иначе, если рассматриваем funding currency (например, fUSD), то поля объекта заполняем немного по-другому
                if (!isFundingCurrency)
                {
                    trade.Id = splittedByComaArray[0];
                    trade.Time = DateTimeOffset.FromUnixTimeMilliseconds(long.Parse(splittedByComaArray[1]));
                    trade.Pair = splittedByComaArray[2];
                    trade.Price = decimal.Parse(splittedByComaArray[3], formatter);
                    trade.Rate = null;
                    trade.Period = null;
                }
                else
                {
                    trade.Id = splittedByComaArray[0];
                    trade.Time = DateTimeOffset.FromUnixTimeMilliseconds(long.Parse(splittedByComaArray[1]));
                    trade.Pair = splittedByComaArray[2];
                    trade.Price = null;
                    trade.Rate = float.Parse(splittedByComaArray[3], formatter);
                    trade.Period = int.Parse(splittedByComaArray[4]);
                }

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
                    Open = double.Parse(splittedByComaArray[1], formatter),
                    Close = double.Parse(splittedByComaArray[2], formatter),
                    High = double.Parse(splittedByComaArray[3], formatter),
                    Low = double.Parse(splittedByComaArray[4], formatter),
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
