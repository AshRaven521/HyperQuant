using HyperQuantConnector.Models;

namespace HyperQuantConnector
{
    public class JSONParser
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
    }
}
