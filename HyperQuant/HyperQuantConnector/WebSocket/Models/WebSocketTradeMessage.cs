using System.Text.Json.Serialization;

namespace HyperQuantConnector.WebSocket.Models
{
    public class WebSocketTradeMessage
    {

        [JsonPropertyName("event")]
        public string EventType { get; set; }

        [JsonPropertyName("channel")]
        public string Channel { get; set; }

        [JsonPropertyName("symbol")]
        public string Symbol { get; set; }


    }
}
