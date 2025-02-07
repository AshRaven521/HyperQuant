namespace HyperQuantConnector.Models.Crypto
{
    /// <summary>
    /// Портфель с кол-вом криптовалюты
    /// </summary>
    public class CryptoBriefCase
    {

        public decimal USDTAmount { get; }
        public decimal BTCAmount { get; }
        public decimal XRPAmount { get; }
        public decimal XMRAmount { get; }
        public decimal DASHAmount { get; }

        public CryptoBriefCase(decimal uSDT, decimal bTC, decimal xRP, decimal xMR, decimal dASH)
        {
            USDTAmount = uSDT;
            BTCAmount = bTC;
            XRPAmount = xRP;
            XMRAmount = xMR;
            DASHAmount = dASH;
        }

        public CryptoBriefCase()
        {

        }
    }
}
