using System;

namespace PumpkinTrade.Models
{
    internal class PumpkinDeal
    {
        public DateTime SubmitionTime { get; set; }
        public string Seller { get; set; }
        public string Buyer { get; set; }
        public decimal? SellPrice { get;  set; }
        public decimal? BuyPrice { get;  set; }
        public EDealType? DealType { get;  set; }
        public decimal? DealPrice { get; set; }
        public DateTime? DealTime { get; set; }
    }
}
