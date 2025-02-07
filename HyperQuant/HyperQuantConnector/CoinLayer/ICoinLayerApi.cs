using HyperQuantConnector.Models.Crypto;

namespace HyperQuantConnector.CoinLayer
{
    public interface ICoinLayerApi
    {
        Task<IDictionary<string, decimal>> GetExchangeRates(string startCurrency, IEnumerable<string> neededCurrencies, string configFilePath);
        IDictionary<string, decimal> CalculateCurrencyCount(IEnumerable<CryptoCurrency> currencies);
    }
}