using System.Linq.Expressions;
using ERPSystem.Domain.Entities;
using ERPSystem.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ERPSystem.Infrastructure.Repositories;

public class Repository<T>(AppDBContext _context) : IRepository<T>
    where T : BaseEntity
{
   

    public IQueryable<T> GetAll(Expression<Func<T, bool>> predicate = null)
    {
        if (predicate != null)
        {
            return _context.Set<T>().Where(predicate);
        }
        return _context.Set<T>();
    }

    public Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate = null)
    {
        if (predicate != null)
        {
            return _context.Set<T>().FirstOrDefaultAsync(predicate);

        }
        return _context.Set<T>().FirstOrDefaultAsync();
    }

    public Guid Add(T entity)
    {
        _context.Set<T>().Add(entity);
        return entity.Id;
    }

    public T Update(T entity)
    {
       _context.Set<T>().Update(entity);
       entity.UpdatedAt = DateTime.Now;
       return entity;
    }

    public void Delete(Guid id)
    {
        var entity = _context.Set<T>().Find(id);
        entity.IsDeleted = true;
        entity.DeletedAt = DateTime.Now;
        _context.Set<T>().Update(entity);
     
    }

    public  Task<int> SaveChangesAsync(CancellationToken cancellationToken)
    {
        return _context.SaveChangesAsync(cancellationToken);
    }
}

