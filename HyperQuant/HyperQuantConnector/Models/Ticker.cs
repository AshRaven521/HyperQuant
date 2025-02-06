namespace HyperQuantConnector.Models
{
    public class Ticker
    {
        public double? FRR { get; set; }
        public double BID { get; set; }
        public int? BID_PERIOD { get; set; }
        public double BID_SIZE { get; set; }
        public double ASK { get; set; }
        public int? ASK_PERIOD { get; set; }
        public double ASK_SIZE { get; set; }
        public double DAILY_CHANGE { get; set; }
        public double DAILY_CHANGE_PERC { get; set; }
        public double LAST_PRICE { get; set; }
        public double VOLUME { get; set; }
        public double HIGH { get; set; }
        public double LOW { get; set; }
        public double? FRR_AMOUNT_AVAILABLE { get; set; }
    }
}
