using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DAL.Repository
{
    public interface IRepository<T> : IDisposable where T : class                                                 
    {
        void Create(T item);
        void Remove(T item);
        void Update(T item);
        IEnumerable<T> Find(Func<T, Boolean> predicate);
        IQueryable<T> FindWithExpressionsTree(Expression<Func<T, Boolean>> predicate);
        bool Any(Expression<Func<T, Boolean>> predicate);
        Task<bool> AnyAsync(Expression<Func<T, Boolean>> predicate);
        int Count(Expression<Func<T, Boolean>> predicate);
        IEnumerable<T> GetAll();
        IQueryable<T> GetAllWithExpressionsTree();
    }
}
