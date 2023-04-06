// Copyright © CompanyName. All Rights Reserved.
using Example.DataTransfer;
using Example.Repository;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Example.RepositoryHandler.MsSql.EF.CoreSql
{
    public class BaseRepositoryOperations<T> : IAsyncRepositoryOperations<T>
        where T : BaseEntity
    {
        private readonly ApplicationDbContext applicationDbContext;
        private readonly DbSet<T> dbSet;
        public BaseRepositoryOperations(ApplicationDbContext exampleDbContext)
        {
            this.applicationDbContext = exampleDbContext ?? throw new ArgumentNullException(nameof(exampleDbContext));
            this.dbSet = applicationDbContext.Set<T>();
        }

        public ApplicationDbContext ApplicationDbContext
        {
            get => this.applicationDbContext;
        }

        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            return await applicationDbContext.Set<T>().ToListAsync().ConfigureAwait(false);
        }

        public async Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate)
        {
            return await applicationDbContext.Set<T>().Where(predicate).ToListAsync().ConfigureAwait(false);
        }

        public async Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate = null, Func<IQueryable<T>,
            IOrderedQueryable<T>> orderBy = null, string includeString = null, bool disableTracking = true)
        {
            IQueryable<T> query = applicationDbContext.Set<T>();
            if (disableTracking)
            {
                query = query.AsNoTracking();
            }

            if (!string.IsNullOrWhiteSpace(includeString))
            {
                query = query.Include(includeString);
            }

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            if (orderBy != null)
            {
                return await orderBy(query).ToListAsync().ConfigureAwait(false);
            }

            return await query.ToListAsync().ConfigureAwait(false);
        }

        public async Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate = null, Func<IQueryable<T>,
            IOrderedQueryable<T>> orderBy = null, List<Expression<Func<T, object>>> includes = null, bool disableTracking = true)
        {
            IQueryable<T> query = applicationDbContext.Set<T>();
            if (disableTracking)
            {
                query = query.AsNoTracking();
            }

            if (includes != null)
            {
                query = includes.Aggregate(query, (current, include) => current.Include(include));
            }

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            if (orderBy != null)
            {
                return await orderBy(query).ToListAsync().ConfigureAwait(false);
            }

            return await query.ToListAsync().ConfigureAwait(false);
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await applicationDbContext.Set<T>().FindAsync(id);
        }

        public async Task<T> AddAsync(T entity)
        {
            applicationDbContext.Set<T>().Add(entity);
            await applicationDbContext.SaveChangesAsync().ConfigureAwait(false);
            return entity;
        }

        public async Task UpdateAsync(T entity)
        {
            applicationDbContext.ChangeTracker.Clear();
            applicationDbContext.Entry(entity).State = EntityState.Modified;
            await applicationDbContext.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task DeleteAsync(T entity)
        {
            applicationDbContext.Set<T>().Remove(entity);
            await applicationDbContext.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task DeleteByIdAsync(int id)
        {
            var entity = await applicationDbContext.Set<T>().FindAsync(id);
            if (entity != null)
            {
                applicationDbContext.Set<T>().Remove(entity);
                await applicationDbContext.SaveChangesAsync().ConfigureAwait(false);
            }
        }

        public async Task<int> UpdateRangeAsync(IList<T> entity)
        {
            applicationDbContext.UpdateRange(entity);
            return await applicationDbContext.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task<int> AddRangeAsync(IList<T> entity)
        {
            applicationDbContext.Set<T>().AddRange(entity);
            return await applicationDbContext.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task<int> DeleteRangeAsync(IList<T> entity)
        {
            applicationDbContext.Set<T>().RemoveRange(entity);
            return await applicationDbContext.SaveChangesAsync().ConfigureAwait(false);
        }
        public async Task ChangeDeletedStatusAsync(int id)
        {
            var entity = await ApplicationDbContext.Set<T>().FindAsync(id);
            if (entity != null)
            {
                // entity.IsDeleted = (entity.IsDeleted == ((char)AppConstants.IsDeleted.No) ? ((char)AppConstants.IsDeleted.Yes) : ((char)AppConstants.IsDeleted.No));
                ApplicationDbContext.Set<T>().Update(entity);
                await ApplicationDbContext.SaveChangesAsync().ConfigureAwait(false);
            }
        }

        public IEnumerable<T> ExecuteCommandQuery(string command)
        {
           return dbSet.FromSqlRaw(command);
        }
    }
}
