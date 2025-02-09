using HyperQuantConnector.Models;

namespace HyperQuantConnector.WebSocket
{
    public interface IWebSocketClient
    {
        event Action<Trade> NewBuyTrade;
        event Action<Trade> NewSellTrade;
        IEnumerable<Trade> ParsedTrades { get; }
        Task SubscribeTrades(string pair, int maxCount = 100);
        void UnsubscribeTrades(string pair);

        event Action<Candle> CandleSeriesProcessing;
        Task SubscribeCandles(string pair, int periodInSec, DateTimeOffset? from = null, DateTimeOffset? to = null, long? count = 0);
        void UnsubscribeCandles(string pair);
    }
}
