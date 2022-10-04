using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using SQLite;

namespace Repository
{
    public class RepositoryBase<T>: IRepositoryBase<T> where T:class, new()
    {
        private SQLiteAsyncConnection databaseConnection; 

        public RepositoryBase(SQLiteAsyncConnection databaseConnection)
        {
            this.databaseConnection = databaseConnection; 
        }

        public async Task<List<T>> Get()
        {
            return await databaseConnection.Table<T>().ToListAsync();
        }

        public async Task<T> Get(int id)
        {
            return await databaseConnection.FindAsync<T>(id);
        }

        public async Task<List<T>> Get<TValue>(Expression<Func<T, bool>> predicate = null, Expression<Func<T, TValue>> orderBy = null)
        {
            var query = databaseConnection.Table<T>();

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            if (orderBy != null)
            {
                query = query.OrderBy(orderBy);
            }

            return await query.ToListAsync();
        }

        public async Task<T> Get(Expression<Func<T, bool>> predicate)
        {
            return await databaseConnection.FindAsync<T>(predicate);
        }

        public AsyncTableQuery<T> AsQueryable()
        {
            return databaseConnection.Table<T>();
        }

        public async Task<int> Create(T entity)
        {
            if (databaseConnection.FindAsync<T>(entity) == null)
            return await databaseConnection.InsertAsync(entity);

            return 0; 
        }

        public async Task<int> Update(T entity)
        {
            return await databaseConnection.UpdateAsync(entity);
        }

        public async Task<int> Delete(T entity)
        {
            return await databaseConnection.DeleteAsync(entity);
        }
    }
}
