namespace HyperQuantConnector.Models
{
    public class Candle
    {

        /* Пояснение! Оставляю поля в соответствии с документацией: https://docs.bitfinex.com/reference/rest-public-candles */

        /// <summary>
        /// Время
        /// </summary>
        public DateTimeOffset OpenTime { get; set; }
        /// <summary>
        /// First execution during the time frame
        /// </summary>
        public double Open { get; set; }
        /// <summary>
        /// Last execution during the time frame
        /// </summary>
        public double Close { get; set; }
        /// <summary>
        /// Highest execution during the time frame
        /// </summary>
        public double High { get; set; }
        /// <summary>
        /// Lowest execution during the timeframe
        /// </summary>
        public double Low { get; set; }
        /// <summary>
        /// Quantity of symbol traded within the timeframe
        /// </summary>
        public double Volume { get; set; }



    }
}
