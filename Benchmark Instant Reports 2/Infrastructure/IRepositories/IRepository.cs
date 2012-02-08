using System;
using System.Linq;

namespace Benchmark_Instant_Reports_2.Infrastructure.IRepositories
{
    /// <summary>
    /// interface definition for the base repository members and methods;
    /// used as a base class for all repositories
    /// </summary>
    /// <typeparam name="T">Type of Entity of the repository</typeparam>
    public interface IRepository<T> where T : class
    {
        IQueryable<T> FindAll();
        IQueryable<T> FindWhere(Func<T, bool> predicate);

        void Add(T newentity);
        void Remove(T entity);
    }
}