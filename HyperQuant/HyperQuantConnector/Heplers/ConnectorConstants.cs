namespace HyperQuantConnector.Heplers
{
    public class ConnectorConstants
    {

        public const string DEFAULT_PERIOD_QUERY_PARAM = "1m";

        public static Dictionary<string, int> QueryPeriodInSeconds { get; } = new Dictionary<string, int>()
        {
            {"1m", 60 },
            {"5m", 300 },
            {"15m", 900 },
            {"30m", 1800 },
            {"1h", 3600 },
            {"3h", 10800 },
            {"6h", 21600 },
            {"12h", 43200 },
            {"1D", 86400 },
            {"1W", 604800 },
            {"14D", 1209600 },
            {"1M", 2419200 },

        };
    }
}
