using System.Text.Json.Serialization;

namespace HyperQuantConnector.CoinLayer
{
    public class ApiKey
    {
        [JsonPropertyName("CoinLayerApi")]
        public string Key { get; set; }
    }
}
