using System;
using System.Linq;
using PumpkinTrade.Extensions;
using PumpkinTrade.Models;
using PumpkinTrade.Repository;

namespace PumpkinTrade
{
    internal class PumpkinTradeService : IPumpkinTradeService
    {
        private static readonly object AddLock = new object();
        private readonly IRepository<PumpkinDeal> _dataSource;

        internal PumpkinTradeService(IRepository<PumpkinDeal> dataSource)
        {
            _dataSource = dataSource;
        }

        public bool Buy(string userName, decimal price)
        {
            userName = string.IsNullOrWhiteSpace(userName) ? "Unnamed buyer" : userName;

            var suitableProposal = GetSuitableProposal(price);

            if (suitableProposal == null)
            {
                return AddBuyDealToDataSource(userName, price);
            }

            return ThreadSafeBuy(suitableProposal, userName, price);
        }

        public bool Sell(string userName, decimal price)
        {
            userName = string.IsNullOrWhiteSpace(userName) ? "Unnamed seller" : userName;

            var suitableRequest = GetSuitableRequest(price);

            if (suitableRequest == null)
            {
                return AddSellDealToDataSource(userName, price);
            }

            return ThreadSafeSell(suitableRequest, userName, price);
        }

        public string GetProcessedTrades()
        {
            return string.Join("\n",
                _dataSource.Get().Where(d => d.DealType.HasValue && d.DealPrice.HasValue).OrderBy(d => d.DealTime).AsParallel()
                    .Select(d => d.GetDealResult()));
        }

        private PumpkinDeal GetSuitableProposal(decimal price)
        {
            return _dataSource.Get().Where(d => d.BuyPrice == null && d.SellPrice <= price).OrderBy(d => d.SellPrice)
                .ThenBy(d => d.SubmitionTime).FirstOrDefault();
        }

        private PumpkinDeal GetSuitableRequest(decimal price)
        {
            return _dataSource.Get().Where(d => d.SellPrice == null && d.BuyPrice >= price)
                .OrderByDescending(d => d.BuyPrice)
                .ThenBy(d => d.SubmitionTime).FirstOrDefault();
        }

        private bool AddBuyDealToDataSource(string userName, decimal price)
        {
            lock (AddLock)
            {
                if (GetSuitableProposal(price) == null)
                {
                    _dataSource.Add(new PumpkinDeal()
                    {
                        Buyer = userName,
                        BuyPrice = price,
                        SubmitionTime = DateTime.UtcNow
                    });
                    return false;
                }

                return Buy(userName, price);
            }
        }

        private bool AddSellDealToDataSource(string userName, decimal price)
        {
            lock (AddLock)
            {
                if (GetSuitableRequest(price) == null)
                {
                    _dataSource.Add(new PumpkinDeal()
                    {
                        Seller = userName,
                        SellPrice = price,
                        SubmitionTime = DateTime.UtcNow
                    });
                    return false;
                }

                return Sell(userName, price);
            }
        }

        private bool ThreadSafeBuy(PumpkinDeal deal, string userName, decimal price)
        {
            lock (deal)
            {
                if (deal.BuyPrice == null)
                {
                    deal.Buyer = userName;
                    deal.BuyPrice = price;
                    deal.DealPrice = price;
                    deal.DealType = EDealType.Buy;
                    deal.DealTime = DateTime.UtcNow;
                    return true;
                }

                return Buy(userName, price);
            }
        }

        private bool ThreadSafeSell(PumpkinDeal deal, string userName, decimal price)
        {
            lock (deal)
            {
                if (deal.SellPrice == null)
                {
                    deal.Seller = userName;
                    deal.SellPrice = price;
                    deal.DealPrice = price;
                    deal.DealType = EDealType.Sell;
                    deal.DealTime = DateTime.UtcNow;
                    return true;
                }

                return Sell(userName, price);
            }
        }
    }
}