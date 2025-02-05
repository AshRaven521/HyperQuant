using HyperQuantConnector.Models;
using System.Globalization;
using System.Text.RegularExpressions;

namespace HyperQuantConnector
{
    public class CustomConverter
    {
        /* Парсим вручную ответ с API Bitnefix при запросе данных на пару валют */
        public static IEnumerable<Trade> ParseTrades(string json, bool isFundingCurrency = false)
        {
            var dataArray = json.Replace(']', ',');
            var splittedDataArray = dataArray.Split('[');

            List<Trade> result = new List<Trade>();

            foreach (string splittedData in splittedDataArray)
            {
                if (string.IsNullOrWhiteSpace(splittedData))
                {
                    continue;
                }
                var splittedByComaArray = splittedData.Split(',');
                // Если в массиве есто хоть одна строка, в которой есть хоть одна буква, то пропускаем
                if (splittedByComaArray.Any(str => Regex.Matches(str, @"[a-zA-Z]").Any()))
                {
                    continue;
                }

                //Приходит строка, в которой после парсинга на данный момент выполнения метода получается 2 Trade в одной строке, а парсится как 1. Сделаю проверку, чтобы только эту строку распарсить как 2 Trade
                if (splittedByComaArray.Length > 10)
                {
                    //var firstPart = splittedByComaArray.TakeWhile(x => !string.IsNullOrWhiteSpace(x)).ToArray();
                    var firstPart = splittedByComaArray.Take(splittedByComaArray.Length / 2).ToArray();
                    Trade firstTrade = CreateTrade(isFundingCurrency, firstPart);
                    result.Add(firstTrade);

                    //var secondPart = splittedByComaArray.SkipWhile(x => !string.IsNullOrWhiteSpace(x)).ToArray();
                    var secondPart = splittedByComaArray.Skip(splittedByComaArray.Length / 2).ToArray();
                    Trade secondTrade = CreateTrade(isFundingCurrency, secondPart);
                    result.Add(secondTrade);
                    continue;
                }

                Trade trade = CreateTrade(isFundingCurrency, splittedByComaArray);

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

        private static Trade CreateTrade(bool isCurrency, string[] data)
        {
            Trade trade = new Trade();
            IFormatProvider formatter = new NumberFormatInfo { NumberDecimalSeparator = "." };
            // Если рассматриваем пару валют (например, tBTCUSD). Иначе, если рассматриваем funding currency (например, fUSD), то поля объекта заполняем немного по-другому
            if (!isCurrency)
            {
                trade.Id = data[0];
                trade.Time = DateTimeOffset.FromUnixTimeMilliseconds(long.Parse(data[1]));
                trade.Pair = data[2];
                trade.Price = decimal.Parse(data[3], formatter);
                trade.Rate = null;
                trade.Period = null;
            }
            else
            {
                trade.Id = data[0];
                trade.Time = DateTimeOffset.FromUnixTimeMilliseconds(long.Parse(data[1]));
                trade.Pair = data[2];
                trade.Price = null;
                trade.Rate = float.Parse(data[3], formatter);
                trade.Period = int.Parse(data[4]);
            }

            return trade;
        }


    }
}
