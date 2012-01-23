using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Benchmark_Instant_Reports_2.Infrastructure.IRepositories
{
    public interface IRepository<T> where T : class
    {
        IQueryable<T> FindAll();
        IQueryable<T> FindWhere(Func<T, bool> predicate);

        void Add(T newentity);
        void Remove(T entity);
    }
}