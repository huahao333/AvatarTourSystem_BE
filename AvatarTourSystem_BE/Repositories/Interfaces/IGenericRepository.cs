﻿    using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Interfaces
{
    public interface IGenericRepository<T> : IGenericCommandRepository<T>,
           IGenericQueryRepository<T>
           where T : class
    {
    }

    public interface IGenericCommandRepository<T> where T : class
    {
        Task<T> AddAsync(T entity);
        Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities);
        Task<T> UpdateAsync(T entity);
        Task UpdateRangeAsync(IEnumerable<T> entities);
        Task<T> DeleteAsync(T entity);
    }

    public interface IGenericQueryRepository<T> where T : class
    {
        Task<T> GetByIdAsync(int id);
        Task<T> GetByIdGuidAsync(Guid id);
        Task<IEnumerable<T>> GetByConditionAsync(
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string includeProperties = "",
            int? pageIndex = null,
            int? pageSize = null
        );
        Task<IEnumerable<T>> GetAllAsync();
        Task<int> CountAsync(Expression<Func<T, bool>> filter = null);
    }
}
