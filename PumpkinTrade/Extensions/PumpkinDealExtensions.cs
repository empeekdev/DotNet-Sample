using System.Globalization;
using PumpkinTrade.Models;

namespace PumpkinTrade.Extensions
{
    internal static class PumpkinDealExtensions
    {
        internal static string GetDealResult(this PumpkinDeal deal)
        {
            return deal.DealType == EDealType.Buy
                ? $"{deal.Buyer} bought a pumpkin from {deal.Seller} for {deal.DealPrice.Value.ToString("C0",new CultureInfo("fr-FR"))}"
                : $"{deal.Seller} sold a pumpkin to {deal.Buyer} for {deal.DealPrice.Value.ToString("C0", new CultureInfo("fr-FR"))}";
        }
    }
}