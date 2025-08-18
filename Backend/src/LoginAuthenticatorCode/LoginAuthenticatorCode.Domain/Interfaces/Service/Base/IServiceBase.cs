using FluentValidation.Results;
using System.Linq.Expressions;


namespace LoginAuthenticatorCode.Domain.Interfaces.Service.Base;

public interface IServiceBase<TEntity> where TEntity : class
{
    Task<TEntity> AddAsync(TEntity entity);
    Task AddRangeAsync(IList<TEntity> entity);
    Task<TEntity> UpdateAsync(TEntity entity);
    Task UpdateRangeAsync(IEnumerable<TEntity> entity);
    Task<TEntity> DeleteAsyncById(long Id);
    Task<TEntity> DeleteAsync(TEntity entity);
    Task<TEntity> GetByIdAsync(long Id);
    Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate);
    Task<IEnumerable<TEntity>> GetAllAsync(TEntity entity);
    Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> predicate);
    Task<List<ValidationFailure>> Validate(TEntity entity);
    Task<List<ValidationFailure>> ValidateUserPermission(TEntity entity);
}

