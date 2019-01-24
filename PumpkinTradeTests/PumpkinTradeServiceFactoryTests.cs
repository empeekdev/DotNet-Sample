using NUnit.Framework;
using PumpkinTrade;
using PumpkinTrade.Factories;

namespace PumpkinTradeTests
{
    [TestFixture]
    public class PumpkinTradeServiceFactoryTests
    {
        [Test]
        public void GetPumpkinTradeService_InstanceReturned()
        {
            var result = PumpkinTradeServiceFactory.GetPumpkinTradeService();

            Assert.Multiple(() =>
            {
                Assert.IsNotNull(result);
                Assert.IsInstanceOf<IPumpkinTradeService>(result);
            });
        }
    }
}