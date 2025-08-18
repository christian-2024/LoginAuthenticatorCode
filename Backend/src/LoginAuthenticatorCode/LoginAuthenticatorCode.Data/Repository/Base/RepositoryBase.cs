using LoginAuthenticatorCode.Data.Context;
using LoginAuthenticatorCode.Domain.Entities.Base;
using LoginAuthenticatorCode.Domain.Enum;
using LoginAuthenticatorCode.Domain.Interfaces.Repository.Base;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace LoginAuthenticatorCode.Data.Repository.Base;

    public abstract class RepositoryBase<TEntity> : IRepositoryBase<TEntity> where TEntity : class
    {
    protected readonly LoginAuthenticatorContext _loginAuthenticatorContext;
    protected readonly DbSet<TEntity> DbSet;

    public RepositoryBase(LoginAuthenticatorContext loginAuthenticatorContext)
    {
        _loginAuthenticatorContext = loginAuthenticatorContext;
        DbSet = loginAuthenticatorContext.Set<TEntity>();
    }

    public virtual async Task<TEntity> AddAsync(TEntity entity)
    {
        if (entity is EntityBase entityBase)
        {
            entityBase.DateCreated = DateTime.Now;
        }
        await _loginAuthenticatorContext.Set<TEntity>().AddAsync(entity);
        await _loginAuthenticatorContext.SaveChangesAsync();

        return entity;
    }

    public virtual async Task AddRangeAsync(IList<TEntity> entities)
    {
        _ = entities ?? throw new ArgumentNullException(nameof(entities));

        var insertEntities = new List<TEntity>();
        foreach (var entity in entities)
        {
            if (entity is EntityBase entityBase)
            {
                entityBase.Situation = Situation.Active;
                entityBase.DateCreated = DateTime.Now;

            }
            insertEntities.Add(entity);
        }

        await DbSet.AddRangeAsync(insertEntities);
        await _loginAuthenticatorContext.SaveChangesAsync();
    }

    public async Task<TEntity> DeleteAsync(TEntity entity)
    {
        _ = entity ?? throw new ArgumentNullException(nameof(entity));

        if (entity is EntityBase entityBase)
        {
            entityBase.DateDeleted = DateTime.Now;
            entityBase.Situation = Situation.Deleted;
        }

        _loginAuthenticatorContext.Entry(entity).State = EntityState.Modified;
        await _loginAuthenticatorContext.SaveChangesAsync();
        return entity;
    }

    public virtual async Task<TEntity> DeleteAsyncById(long Id)
    {

        var entityId = await _loginAuthenticatorContext.Set<TEntity>().FindAsync(Id);
        _ = entityId ?? throw new InvalidOperationException($"Não foi possivel encontrar o Id {Id}");

        _loginAuthenticatorContext.Set<TEntity>().Remove(entityId);
        await _loginAuthenticatorContext.SaveChangesAsync();

        return entityId;
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate)
    {
        var result = await DbSet.AsNoTracking().Where(predicate).ToListAsync();
        return result;
    }
    public async Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate, string[] includes)
    {
        IQueryable<TEntity> query = DbSet.AsQueryable();
        if (predicate != null)
        {
            foreach (var include in includes)
            {
                query = query.Include(predicate);
            }
        }
        if (includes != null && includes.Any())
        {
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
        }

        var result = await query.ToListAsync();
        return result;
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync(TEntity entities)
    {
        var result = await DbSet.AsNoTracking().ToListAsync();
        return result;

    }

    public async Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> predicate)
    {
        var result = await DbSet.AsNoTracking().Where(predicate).FirstOrDefaultAsync();
        return result;
    }

    public virtual async Task<TEntity> GetByIdAsync(long Id)
    {
        var entityId = await _loginAuthenticatorContext.Set<TEntity>().FindAsync(Id);
        return entityId;
    }

    public async Task<TEntity> UpdateAsync(TEntity entity)
    {
        _ = entity ?? throw new ArgumentNullException(nameof(entity));

        if (entity is EntityBase entityBase)
        {
            entityBase.DateModified = DateTime.Now;
        }

        _loginAuthenticatorContext.Entry(entity).State = EntityState.Modified;
        await _loginAuthenticatorContext.SaveChangesAsync();
        return entity;
    }

    public async Task UpdateRangeAsync(IEnumerable<TEntity> entities)
    {
        _ = entities ?? throw new ArgumentNullException(nameof(entities));

        var updateEntities = new List<TEntity>();
        foreach (var entity in entities)
        {
            if (entity is EntityBase entityBase)
            {
                entityBase.DateModified = DateTime.Now;
            }
            _loginAuthenticatorContext.Entry(entity).State = EntityState.Modified;
            updateEntities.Add(entity);
        }

        DbSet.UpdateRange(updateEntities);
        await _loginAuthenticatorContext.SaveChangesAsync();
    }
}
    

