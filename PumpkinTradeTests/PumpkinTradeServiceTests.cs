using System;
using System.Collections.Generic;
using System.Linq;
using NSubstitute;
using NUnit.Framework;
using PumpkinTrade;
using PumpkinTrade.Models;
using PumpkinTrade.Repository;

namespace PumpkinTradeTests
{
    [TestFixture]
    public class PumpkinTradeServiceTests
    {
        [Test]
        public void Buy_NoProposals_TradeNotProcessed_Test()
        {
            var dataSource = Substitute.For<IRepository<PumpkinDeal>>();
            dataSource.Get().Returns(
                new List<PumpkinDeal>()
                    .AsQueryable());
            var target = new PumpkinTradeService(dataSource);

            var result = target.Buy("Test buyer", 8);

            Assert.False(result);
        }

        [Test]
        public void Buy_PriceLessThenProposal_TradeNotProcessed_Test()
        {
            var dataSource = Substitute.For<IRepository<PumpkinDeal>>();
            dataSource.Get().Returns(
                new List<PumpkinDeal>()
                {
                    new PumpkinDeal()
                    {
                        Buyer = null,
                        BuyPrice = null,
                        Seller = "Test seller",
                        SellPrice = 10,
                        DealPrice = null,
                        DealType = null,
                        SubmitionTime = DateTime.UtcNow
                    }
                }.AsQueryable());
            var target = new PumpkinTradeService(dataSource);

            var result = target.Buy("Test buyer", 8);

            Assert.False(result);
        }

        [Test]
        public void Buy_PriceMuchThenProposal_TradeProcessed_Test()
        {
            var dataSource = Substitute.For<IRepository<PumpkinDeal>>();
            dataSource.Get().Returns(
                new List<PumpkinDeal>()
                {
                    new PumpkinDeal()
                    {
                        Buyer = null,
                        BuyPrice = null,
                        Seller = "Test seller",
                        SellPrice = 10,
                        DealPrice = null,
                        DealType = null,
                        SubmitionTime = DateTime.UtcNow
                    }
                }.AsQueryable());
            var target = new PumpkinTradeService(dataSource);

            var result = target.Buy("Test buyer", 18);

            Assert.True(result);
        }

        [Test]
        public void Sell_NoRequests_TradeNotProcessed_Test()
        {
            var dataSource = Substitute.For<IRepository<PumpkinDeal>>();
            dataSource.Get().Returns(
                new List<PumpkinDeal>()
                    .AsQueryable());
            var target = new PumpkinTradeService(dataSource);

            var result = target.Sell("Test seller", 8);

            Assert.False(result);
        }

        [Test]
        public void Sell_PriceLessThenRequest_TradeProcessed_Test()
        {
            var dataSource = Substitute.For<IRepository<PumpkinDeal>>();
            dataSource.Get().Returns(
                new List<PumpkinDeal>()
                {
                    new PumpkinDeal()
                    {
                        Buyer = "Test buyer",
                        BuyPrice = 10,
                        Seller = null,
                        SellPrice = null,
                        DealPrice = null,
                        DealType = null,
                        SubmitionTime = DateTime.UtcNow
                    }
                }.AsQueryable());
            var target = new PumpkinTradeService(dataSource);

            var result = target.Sell("Test seller", 8);

            Assert.True(result);
        }

        [Test]
        public void Sell_PriceMuchThenRequest_TradeNotProcessed_Test()
        {
            var dataSource = Substitute.For<IRepository<PumpkinDeal>>();
            dataSource.Get().Returns(
                new List<PumpkinDeal>()
                {
                    new PumpkinDeal()
                    {
                        Buyer = "Test buyer",
                        BuyPrice = 10,
                        Seller = null,
                        SellPrice = null,
                        DealPrice = null,
                        DealType = null,
                        SubmitionTime = DateTime.UtcNow
                    }
                }.AsQueryable());
            var target = new PumpkinTradeService(dataSource);

            var result = target.Sell("Test seller", 18);

            Assert.False(result);
        }

        [Test]
        public void GetProcessedTrades_OnlyProcessedReturned()
        {
            var dataSource = Substitute.For<IRepository<PumpkinDeal>>();
            dataSource.Get().Returns(
                new List<PumpkinDeal>()
                {
                    new PumpkinDeal()
                    {
                        Buyer = "Test buyer",
                        BuyPrice = 10,
                        Seller = null,
                        SellPrice = null,
                        DealPrice = null,
                        DealType = null,
                        SubmitionTime = DateTime.UtcNow
                    },
                    new PumpkinDeal()
                    {
                        Buyer = null,
                        BuyPrice = null,
                        Seller = "Test seller",
                        SellPrice = 10,
                        DealPrice = null,
                        DealType = null,
                        SubmitionTime = DateTime.UtcNow
                    },
                    new PumpkinDeal()
                    {
                        Buyer = "Test buyer",
                        BuyPrice = 10,
                        Seller = "Test seller",
                        SellPrice = 8,
                        DealPrice = 10,
                        DealType = EDealType.Buy,
                        SubmitionTime = DateTime.UtcNow
                    },
                    new PumpkinDeal()
                    {
                        Buyer = "Test buyer",
                        BuyPrice = 10,
                        Seller = "Test seller",
                        SellPrice = 8,
                        DealPrice = 8,
                        DealType = EDealType.Sell,
                        SubmitionTime = DateTime.UtcNow
                    }
                }.AsQueryable());
            var target = new PumpkinTradeService(dataSource);

            var result = target.GetProcessedTrades();

            Assert.True(result.Split('\n').Length == 2);
            Console.Write(result);
        }

        [Test]
        public void Buy_PriceMatchingProposal_DealMadeAsBuyWithBuyerPrice_Test()
        {
            var targetProposal = new PumpkinDeal()
            {
                Buyer = null,
                BuyPrice = null,
                Seller = "Test seller",
                SellPrice = 10,
                DealPrice = null,
                DealType = null,
                SubmitionTime = DateTime.UtcNow
            };
            var dataSource = Substitute.For<IRepository<PumpkinDeal>>();
            dataSource.Get().Returns(
                new List<PumpkinDeal>()
                {
                    targetProposal
                }.AsQueryable());
            var target = new PumpkinTradeService(dataSource);

            var result = target.Buy("Test buyer", 18);

            Assert.Multiple(() =>
            {
                Assert.True(result);
                Assert.True(targetProposal.DealType == EDealType.Buy);
                Assert.True(targetProposal.BuyPrice == targetProposal.DealPrice);
            });

            Console.Write(target.GetProcessedTrades());
        }

        [Test]
        public void Sell_PriceMatchingRequest_DealMadeAsSellWithSellerPrice_Test()
        {
            var targetRequest = new PumpkinDeal()
            {
                Buyer = "Test buyer",
                BuyPrice = 10,
                Seller = null,
                SellPrice = null,
                DealPrice = null,
                DealType = null,
                SubmitionTime = DateTime.UtcNow
            };
            var dataSource = Substitute.For<IRepository<PumpkinDeal>>();
            dataSource.Get().Returns(
                new List<PumpkinDeal>()
                {
                    targetRequest      
                }.AsQueryable());
            var target = new PumpkinTradeService(dataSource);

            var result = target.Sell("Test seller", 8);

            Assert.Multiple(() =>
            {
                Assert.True(result);
                Assert.True(targetRequest.DealType == EDealType.Sell);
                Assert.True(targetRequest.SellPrice == targetRequest.DealPrice);
            });

            Console.Write(target.GetProcessedTrades());
        }
    }
}