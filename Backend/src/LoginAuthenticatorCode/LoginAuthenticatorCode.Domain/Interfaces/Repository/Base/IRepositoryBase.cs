
using System.Linq.Expressions;

namespace LoginAuthenticatorCode.Domain.Interfaces.Repository.Base;

public interface IRepositoryBase<TEntity> where TEntity : class
{
    Task<TEntity> GetByIdAsync(long id);
    Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> predicate);
    Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate);
    Task<IEnumerable<TEntity>> GetAllAsync(TEntity entities);
    Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate, string[] includes);
    Task<TEntity> AddAsync(TEntity entity);
    Task AddRangeAsync(IList<TEntity> entities);
    Task<TEntity> UpdateAsync(TEntity entity);
    Task UpdateRangeAsync(IEnumerable<TEntity> entities);
    Task<TEntity> DeleteAsyncById(long Id);
    Task<TEntity> DeleteAsync(TEntity entity);

}

