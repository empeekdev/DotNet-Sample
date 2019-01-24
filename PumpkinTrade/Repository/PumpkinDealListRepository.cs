using System.Collections.Generic;
using System.Linq;
using PumpkinTrade.Models;

namespace PumpkinTrade.Repository
{
    internal class PumpkinDealListRepository : IRepository<PumpkinDeal>
    {
        private readonly IList<PumpkinDeal> _data;
        private static readonly object Lock = new object();
        private static IRepository<PumpkinDeal> instance;

        private PumpkinDealListRepository()
        {
            _data = new List<PumpkinDeal>();
        }
        
        internal static IRepository<PumpkinDeal> Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (Lock)
                    {
                        if (instance == null)
                        {
                            CreateAndBindInstance();
                        }
                    }
                }

                return instance;
            }
        }

        internal static IRepository<PumpkinDeal> CreateAndBindInstance()
        {
            instance = new PumpkinDealListRepository();
            return instance;
        }

        public IQueryable<PumpkinDeal> Get()
        {
            return _data.AsQueryable();
        }

        public void Add(PumpkinDeal item)
        {
            _data.Add(item);
        }
    }
}