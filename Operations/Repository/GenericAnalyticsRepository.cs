using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using Operations.Models;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace Operations.Repository
{
    public class GenericAnalyticsRepository<T> : Operations.Contracts.IRepository<T> where T : class
    {
        public AnalyticsEntities DbContext;

        public GenericAnalyticsRepository(AnalyticsEntities dbContext)
        {
            DbContext = dbContext;
        }

        public async Task<int> AddAsync(T t)
        {
            try
            {
                DbContext.Set<T>().Add(t);
                return await DbContext.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                
                throw new Exception(ex.Message + ex.StackTrace);
            }
        }

        public async Task<int> RemoveAsync(T t)
        {
            DbContext.Entry(t).State = EntityState.Deleted;
            return await DbContext.SaveChangesAsync();
        }

        public async Task<int> UpdateAsync(T t)
        {
            DbContext.Entry(t).State = EntityState.Modified;
            return await DbContext.SaveChangesAsync();
        }

        public async Task<int> CountAsync()
        {
            return await DbContext.Set<T>().CountAsync();
        }

        public async Task<List<T>> GetAllAsync()
        {
            try
            {
                var ret = await DbContext.Set<T>().ToListAsync();
                return ret;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<T> FindAsync(Expression<Func<T, bool>> match)
        {
            return await DbContext.Set<T>().SingleOrDefaultAsync(match);
        }

        public async Task<List<T>> FindAllAsync(Expression<Func<T, bool>> match)
        {
            return await DbContext.Set<T>().Where(match).ToListAsync();
            //throw new NotImplementedException();
        }
    }
}