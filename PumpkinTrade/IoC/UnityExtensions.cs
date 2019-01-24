using PumpkinTrade.Factories;
using PumpkinTrade.Models;
using PumpkinTrade.Repository;
using Unity;
using Unity.Injection;
using Unity.Lifetime;

namespace PumpkinTrade.IoC
{
    public static class UnityExtensions
    {
        private static IUnityContainer _container;

        public static IUnityContainer RegisterPumpkinTrade(this IUnityContainer container)
        {
            container.RegisterType<IPumpkinTradeService>(new InjectionFactory(c => PumpkinTradeServiceFactory.GetPumpkinTradeService()));

            _container = container.CreateChildContainer();
            _container.RegisterType<IRepository<PumpkinDeal>>(
                new SingletonLifetimeManager(), new InjectionFactory(c => PumpkinDealListRepository.CreateAndBindInstance()));

            return container;
        }

        internal static T GetService<T>() where T : class
        {
            if (_container != null)
            {
                return _container.IsRegistered<T>() ? _container.Resolve<T>() : null;
            }

            return null;
        }
    }
}