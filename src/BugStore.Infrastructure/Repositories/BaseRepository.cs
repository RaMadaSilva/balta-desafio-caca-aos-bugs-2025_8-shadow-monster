using BugStore.Domain.Contracts.IRepositories;
using BugStore.Domain.Entities;
using BugStore.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BugStore.Infrastructure.Repositories;

public abstract class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : BaseEntity
{
    protected readonly AppDbContext _context;
    public BaseRepository(AppDbContext context) => _context = context;

    public void Add(TEntity entity)
           => _context.Set<TEntity>().Add(entity);

    public void Delete(TEntity entity)
            => _context.Set<TEntity>().Remove(entity);

    public IQueryable<TEntity> GetAll(bool tracking)
         => tracking
            ? _context.Set<TEntity>()
            : _context.Set<TEntity>().AsNoTracking();

    public IQueryable<TEntity> GetByCondition(Expression<Func<TEntity, bool>> expression, bool tracking)
         => tracking
            ? _context.Set<TEntity>().Where(expression)
            : _context.Set<TEntity>().Where(expression).AsNoTracking();

    public void Update(TEntity entity)
         => _context.Set<TEntity>().Update(entity);
}