using Microsoft.EntityFrameworkCore.Query;
using PagedList.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace APIProject.Repository.Interfaces
{


    public interface IRepository<T> where T : class
    {

        #region Async Reponsitory
        Task<T> GetFirstOrDefaultAsync(Expression<Func<T, bool>> predicate, Func<IQueryable<T>,
        IOrderedQueryable<T>> orderBy = null,
        Func<IQueryable<T>, IIncludableQueryable<T, Object>> include = null);
        Task<IList<T>> GetAllAsync(Expression<Func<T, bool>> predicate = null, Func<IQueryable<T>,
        IOrderedQueryable<T>> orderBy = null,
        Func<IQueryable<T>, IIncludableQueryable<T, Object>> include = null);
        Task<IPagedList<T>> GetAllPagedListAsync(int Page, int Limit, Expression<Func<T, bool>> predicate = null, Func<IQueryable<T>,
        IOrderedQueryable<T>> orderBy = null,
        Func<IQueryable<T>, IIncludableQueryable<T, Object>> include = null);
        Task<T> AddAsync(T entity);
        Task<bool> AddManyAsync(IEnumerable<T> entities);
        Task<T> UpdateAsync(T entity);
        Task DeleteAsync(T entity);
    
        #endregion

    }
}
