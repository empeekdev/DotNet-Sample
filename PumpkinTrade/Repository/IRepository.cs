using System.Linq;

namespace PumpkinTrade.Repository
{
    internal interface IRepository<T>
    {
        IQueryable<T> Get();
        void Add(T item);
    }
}