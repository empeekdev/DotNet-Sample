using System;
using NUnit.Framework;
using PumpkinTrade;
using PumpkinTrade.Repository;

namespace PumpkinTradeTests
{
    [TestFixture]
    public class FlowTests
    {
        [Test]
        public void Flow_ExampleProvidedInTaskDescription_Test()
        {
            var dataSource = PumpkinDealListRepository.Instance;
            var target = new PumpkinTradeService(dataSource);

            var result1 = target.Buy("A", 10);
            var result2 = target.Buy("B", 11);
            var result3 = target.Sell("C", 15);
            var result4 = target.Sell("D", 9);
            var result5 = target.Buy("E", 10);
            var result6 = target.Sell("F", 10);
            var result7 = target.Buy("G", 100);

            Assert.Multiple(() =>
            {
                Assert.False(result1);
                Assert.False(result2);
                Assert.False(result3);
                Assert.True(result4);
                Assert.False(result5);
                Assert.True(result6);
                Assert.True(result7);
            });

            Console.Write(target.GetProcessedTrades());
        }
    }
}