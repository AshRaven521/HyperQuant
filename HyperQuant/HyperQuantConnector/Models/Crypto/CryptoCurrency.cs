namespace HyperQuantConnector.Models.Crypto
{
    public class CryptoCurrency
    {
        public CryptoType Type { get; set; }
        public int Count { get; set; }
        public IDictionary<string, decimal>? ExchangeRates { get; set; }
    }
}
