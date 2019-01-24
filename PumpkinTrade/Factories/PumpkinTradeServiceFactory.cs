using PumpkinTrade.IoC;
using PumpkinTrade.Models;
using PumpkinTrade.Repository;

namespace PumpkinTrade.Factories
{
    public static class PumpkinTradeServiceFactory
    {
        public static IPumpkinTradeService GetPumpkinTradeService()
        {
            var dataSource = UnityExtensions.GetService<IRepository<PumpkinDeal>>() ??
                             PumpkinDealListRepository.Instance;

            return new PumpkinTradeService(dataSource);
        }
    }
}