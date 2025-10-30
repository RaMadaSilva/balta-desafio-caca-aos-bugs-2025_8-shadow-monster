using BugStore.Domain.Entities;
using System.Linq.Expressions;

namespace BugStore.Domain.Contracts.IRepositories; 

public interface IBaseRepository<TEntity> where TEntity: BaseEntity
{
    void Add(TEntity entity);
    void Update(TEntity entity);
    void Delete(TEntity entity);
    IQueryable<TEntity> GetByCondition(Expression<Func<TEntity, bool>> expression, bool tracking);
    IQueryable<TEntity> GetAll(bool tracking);
}
