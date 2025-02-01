namespace HyperQuantConnector.Models
{
    public class Trade
    {
        /* Пояснение! Оставляю только поля, которые приходят в ответе на запрос по URI: https://api-pub.bitfinex.com/v2/trades/{pair}/hist?limit={maxCount}&sort=-1. А именно 4 поля КОНКРЕТНО ДЛЯ ПАРЫ! */


        /// <summary>
        /// Id трейда
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Время трейда
        /// </summary>
        public DateTimeOffset Time { get; set; }
        /// <summary>
        /// Валютная пара
        /// </summary>
        public string Pair { get; set; }

        /// <summary>
        /// Цена трейда
        /// </summary>
        public decimal Price { get; set; }



    }
}
