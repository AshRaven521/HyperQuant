using HyperQuantConnector.CoinLayer;
using System.Text.Json;

namespace HyperQuantConnector.Heplers
{
    public static class ConfigurationHelper
    {
        public static string GetApiKey(string filePath)
        {
            string json = File.ReadAllText(filePath);
            var apiKey = JsonSerializer.Deserialize<ApiKey>(json);

            if (string.IsNullOrEmpty(apiKey.Key))
            {
                return string.Empty;
            }
            return apiKey.Key;

        }

    }
}
