using System.Text.Json.Serialization;

namespace HyperQuantConnector.WebSocket.Models
{
    public class WebSocketCandleMessage
    {
        [JsonPropertyName("event")]
        public string EventType { get; set; }

        [JsonPropertyName("channel")]
        public string Channel { get; set; }

        [JsonPropertyName("key")]
        public string Key { get; set; }
    }
}
