
using FluentValidation.Results;
using LoginAuthenticatorCode.Domain.Entities;
using LoginAuthenticatorCode.Domain.Interfaces.Service;
using System.Linq.Expressions;

namespace LoginAuthenticatorCode.Service.Services;

public class UserService : IUserService
{
    public Task<User> AddAsync(User entity)
    {
        throw new NotImplementedException();
    }

    public Task AddRangeAsync(IList<User> entity)
    {
        throw new NotImplementedException();
    }

    public Task<User> DeleteAsync(User entity)
    {
        throw new NotImplementedException();
    }

    public Task<User> DeleteAsyncById(long Id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<User>> GetAllAsync(Expression<Func<User, bool>> predicate)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<User>> GetAllAsync(User entity)
    {
        throw new NotImplementedException();
    }

    public Task<User> GetAsync(Expression<Func<User, bool>> predicate)
    {
        throw new NotImplementedException();
    }

    public Task<User> GetByIdAsync(long Id)
    {
        throw new NotImplementedException();
    }

    public Task<User> UpdateAsync(User entity)
    {
        throw new NotImplementedException();
    }

    public Task UpdateRangeAsync(IEnumerable<User> entity)
    {
        throw new NotImplementedException();
    }

    public Task<List<ValidationFailure>> Validate(User entity)
    {
        throw new NotImplementedException();
    }

    public Task<List<ValidationFailure>> ValidateUserPermission(User entity)
    {
        throw new NotImplementedException();
    }
}

