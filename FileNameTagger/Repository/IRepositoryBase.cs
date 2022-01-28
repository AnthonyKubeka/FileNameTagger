using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using SQLite;

namespace Repository
{
    public interface IRepositoryBase<T> where T:class, new()
    {
        Task<System.Collections.Generic.List<T>> Get();
        Task<T> Get(int id);
        Task<System.Collections.Generic.List<T>> Get<TValue>(Expression<Func<T, bool>> predicate = null, Expression<Func<T, TValue>> orderBy = null);
        Task<T> Get(Expression<Func<T, bool>> predicate);
        AsyncTableQuery<T> AsQueryable();
        Task<int> Create(T entity);
        Task<int> Update(T entity);
        Task<int> Delete(T entity);
    }
}
