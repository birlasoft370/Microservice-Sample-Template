﻿using Example.DataTransfer;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Example.Repository
{
    public interface IAsyncRepositoryOperations<T> where T : BaseEntity
    {
        Task<IReadOnlyList<T>> GetAllAsync();

        Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate);

        Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate = null, Func<IQueryable<T>,
            IOrderedQueryable<T>> orderBy = null, string includeString = null, bool disableTracking = true);

        Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate = null, Func<IQueryable<T>,
            IOrderedQueryable<T>> orderBy = null, List<Expression<Func<T, object>>> includes = null, bool disableTracking = true);

        Task<T> GetByIdAsync(int id);

        Task<T> AddAsync(T entity);

        Task UpdateAsync(T entity);

        Task DeleteAsync(T entity);

        Task DeleteByIdAsync(int id);

        Task<int> UpdateRangeAsync(IList<T> entity);

        Task<int> AddRangeAsync(IList<T> entity);

        Task<int> DeleteRangeAsync(IList<T> entity);

        IEnumerable<T> ExecuteCommandQuery(string command);
        
    }
}
