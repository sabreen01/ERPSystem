using System.Linq.Expressions;
using ERPSystem.Domain.Entities;

namespace ERPSystem.Domain.Interfaces;

public interface IRepository<T>  where T : BaseEntity
{
    IQueryable<T> GetAll(Expression<Func<T,bool>> predicate = null);
    Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate = null);
    Guid Add(T entity);
    T Update(T entity);
    void Delete(Guid id);
    
}