using HyperQuantConnector.Models;
using System.Globalization;
using System.Text.RegularExpressions;

namespace HyperQuantConnector.Heplers
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
                    var firstPart = splittedByComaArray.Take(splittedByComaArray.Length / 2).ToArray();
                    Trade firstTrade = CreateTrade(isFundingCurrency, firstPart);
                    result.Add(firstTrade);

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

        public static IEnumerable<Ticker> ParseTickers(string json, bool isFundingCurrency = false)
        {
            var dataArray = json.Replace(']', ',');
            var splittedDataArray = dataArray.Split('[');

            List<Ticker> result = new List<Ticker>();

            foreach (string splittedData in splittedDataArray)
            {
                if (string.IsNullOrWhiteSpace(splittedData))
                {
                    continue;
                }
                var splittedByComaArray = splittedData.Split(',');
                // Если в массиве есто хоть одна строка, в которой есть хоть одна буква, то пропускаем

                Ticker ticker = CreateTicker(splittedByComaArray, isFundingCurrency);

                
                result.Add(ticker);

            }
            return result;
        }

        public static IEnumerable<Candle> ParseCandles(string json)
        {
            var dataArray = json.Replace(']', ' ');
            var splittedDataArray = dataArray.Split('[');

            List<Candle> result = new List<Candle>();

            int neededFieldsForModelCount = 6;

            foreach (string splittedData in splittedDataArray)
            {
                if (string.IsNullOrWhiteSpace(splittedData))
                {
                    continue;
                }
                var splittedByComaArray = splittedData.Split(',');

                //Проверяю, если получившийся массив меньше кол-ва элементов, требуемых для создания модели Candle(у которой 6 полей), пропускаю итерацию (т.к. получается массив с одним channel_id)
                if (splittedByComaArray.Length < neededFieldsForModelCount)
                {
                    continue;
                }

                IFormatProvider formatter = new NumberFormatInfo { NumberDecimalSeparator = "." };

                if (!long.TryParse(splittedByComaArray[0], out _))
                {
                    continue;
                }
                var candleFiledsWithoutTime = splittedByComaArray.Skip(0).Take(5);
                // Проверяю элементы массива с индексами 1 - 5 на возможность преобразования в double. Если преобразование невозможно, пропускаю итерацию
                if (splittedByComaArray.Skip(0).Take(5).Any(x => !double.TryParse(x, out _)))
                {
                    continue;
                }

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

        private static Ticker CreateTicker(string[] data, bool isFunding)
        {
            Ticker ticker = new Ticker();

            IFormatProvider formatProvider = new NumberFormatInfo { NumberDecimalSeparator = "." };

            if (!isFunding)
            {
                ticker.BID = double.Parse(data[0], formatProvider);
                ticker.BID_SIZE = double.Parse(data[1], formatProvider);
                ticker.ASK = double.Parse(data[2], formatProvider);
                ticker.ASK_SIZE = double.Parse(data[3], formatProvider);
                ticker.DAILY_CHANGE = double.Parse(data[4], formatProvider);
                ticker.DAILY_CHANGE_PERC = double.Parse(data[5], formatProvider);
                ticker.LAST_PRICE = double.Parse(data[6], formatProvider);
                ticker.VOLUME = double.Parse(data[7], formatProvider);
                ticker.HIGH = double.Parse(data[8], formatProvider);
                ticker.LOW = double.Parse(data[9], formatProvider);
                ticker.FRR = null;
                ticker.BID_PERIOD = null;
                ticker.ASK_PERIOD = null;
                ticker.FRR_AMOUNT_AVAILABLE = null;
            }
            // 13 и 14 пункты неизвестно что, даже по документации
            else
            {
                ticker.FRR = double.Parse(data[0], formatProvider);
                ticker.BID = double.Parse(data[1], formatProvider);
                ticker.BID_PERIOD = int.Parse(data[2]);
                ticker.BID_SIZE = double.Parse(data[3], formatProvider);
                ticker.ASK = double.Parse(data[4], formatProvider);
                ticker.ASK_PERIOD = int.Parse(data[5]);
                ticker.ASK_SIZE = double.Parse(data[6], formatProvider);
                ticker.DAILY_CHANGE = double.Parse(data[7], formatProvider);
                ticker.DAILY_CHANGE_PERC = double.Parse(data[8], formatProvider);
                ticker.LAST_PRICE = double.Parse(data[9], formatProvider);
                ticker.VOLUME = double.Parse(data[10], formatProvider);
                ticker.HIGH = double.Parse(data[11], formatProvider);
                ticker.LOW = double.Parse(data[12], formatProvider);
                ticker.FRR_AMOUNT_AVAILABLE = double.Parse(data[15], formatProvider);
            }

            return ticker;
        }

        private static Trade CreateTrade(bool isFunding, string[] data)
        {
            Trade trade = new Trade();
            IFormatProvider formatter = new NumberFormatInfo { NumberDecimalSeparator = "." };
            // Если рассматриваем пару валют (например, tBTCUSD). Иначе, если рассматриваем funding currency (например, fUSD), то поля объекта заполняем немного по-другому
            if (!isFunding)
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
