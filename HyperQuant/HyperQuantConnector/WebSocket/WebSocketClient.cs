using HyperQuantConnector.Heplers;
using HyperQuantConnector.Models;
using HyperQuantConnector.WebSocket.Models;
using System.Net.Sockets;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace HyperQuantConnector.WebSocket
{
    public class WebSocketClient : IWebSocketClient
    {

        public event Action<Trade> NewBuyTrade;
        public event Action<Trade> NewSellTrade;
        public event Action<Candle> CandleSeriesProcessing;
        private IEnumerable<Trade> parsedTrades;
        public IEnumerable<Trade> ParsedTrades 
        {
            get
            {
                return parsedTrades;
            }
        }


        public WebSocketClient() 
        {
            NewBuyTrade += TradeActionHandle;
        }

        private void TradeActionHandle(Trade trade)
        {
            //progress.Report(new TradeEventArgs(trade));
        }

        public async Task SubscribeCandles(string pair, int periodInSec, DateTimeOffset? from = null, DateTimeOffset? to = null, long? count = 0)
        {
            using var socket = new ClientWebSocket();
            await socket.ConnectAsync(new Uri("wss://api-pub.bitfinex.com/ws/2"), CancellationToken.None);

            string periodParam = CustomConverter.GetCandleQueryParamByPeriod(periodInSec);

            StringBuilder key = new StringBuilder();
            if (pair.StartsWith('t'))
            {
                key.AppendLine($"trade:{periodParam}:{pair}");
            }
            else
            {
                key.AppendLine($"trade:{periodParam}:{pair}:a10:p2:p10");
            }

            var message = new WebSocketCandleMessage
            {
                EventType = "subscribe",
                Channel = "candles",
                Key = key.ToString().Replace("\r\n", string.Empty)
            };

            var requestMessageJsonBytes = JsonSerializer.SerializeToUtf8Bytes(message);
            ArraySegment<byte> requestMessageByte = new ArraySegment<byte>(requestMessageJsonBytes);

            await socket.SendAsync(requestMessageByte, WebSocketMessageType.Text, true, CancellationToken.None);

            bool isCandleArrayStarted = false;

            ArraySegment<byte> responseMessageByte = new ArraySegment<byte>(new byte[1024 * 4]);
            var responseBuilder = new StringBuilder();

            while (true)
            {

                var response = await socket.ReceiveAsync(responseMessageByte, CancellationToken.None);

                if (response.MessageType == WebSocketMessageType.Close)
                {
                    await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "", CancellationToken.None);
                }

                if (response.MessageType == WebSocketMessageType.Text)
                {



                    string responseMessage = Encoding.UTF8.GetString(responseMessageByte);
                    // Пропускаем hb сообщения и сообщения со словом event, чтобы в парсер передавать только сообщения с candles
                    if (responseMessage.Contains("hb") || responseMessage.Contains("event"))
                    {
                        continue;
                    }
                    else
                    {
                        //responseBuilder.Append(responseMessage);

                    }

                    //var candles = CustomConverter.ParseCandles(responseBuilder.ToString());
                }
            }
        }

        public async Task SubscribeTrades(string pair, int maxCount = 100)
        {
            using var clientSocket = new ClientWebSocket();
            await clientSocket.ConnectAsync(new Uri("wss://api-pub.bitfinex.com/ws/2"), CancellationToken.None);
            

            var requestMessage = new WebSocketTradeMessage
            {
                EventType = "subscribe",
                Channel = "trades",
                Symbol = pair
            };
                

            var requestMessageJsonBytes = JsonSerializer.SerializeToUtf8Bytes(requestMessage);
            ArraySegment<byte> requestMessageByte = new ArraySegment<byte>(requestMessageJsonBytes);

            await clientSocket.SendAsync(requestMessageByte, WebSocketMessageType.Text, true, CancellationToken.None);

            ArraySegment<byte> responseMessageByte = new ArraySegment<byte>(new byte[1024 * 4]);
            while(true)
            {

                var response = await clientSocket.ReceiveAsync(responseMessageByte, CancellationToken.None);

                if (response.MessageType == WebSocketMessageType.Close)
                {
                    await clientSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "", CancellationToken.None);
                }

                if (response.MessageType == WebSocketMessageType.Text)
                {
                    string responseMessage = Encoding.UTF8.GetString(responseMessageByte);

                    if (responseMessage.Contains("\0"))
                    {
                        responseMessage = responseMessage.Replace("\0", string.Empty);
                    }

                    if (responseMessage.Contains("te") || responseMessage.Contains("fte"))
                    {
                        parsedTrades = CustomConverter.ParseTrades(responseMessage);
                        break;
                    }

                }
            }


        }

        public void UnsubscribeCandles(string pair)
        {
            
        }

        public void UnsubscribeTrades(string pair)
        {
            
        }


    }
}
