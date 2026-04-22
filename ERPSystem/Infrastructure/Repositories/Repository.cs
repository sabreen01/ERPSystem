using System.Linq.Expressions;
using ERPSystem.Domain.Entities;
using ERPSystem.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ERPSystem.Infrastructure.Repositories;

public class Repository<T>(AppDbContext context) : IRepository<T>
    where T : BaseEntity
{
   

    public IQueryable<T> GetAll(Expression<Func<T, bool>> predicate = null)
    {
        if (predicate != null)
        {
            return context.Set<T>().Where(predicate);
        }
        return context.Set<T>();
    }
    
    
    public async Task<T?> GetById(Guid id)
    {
        return await context.Set<T>().FindAsync(id);
    }

    public Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate = null)
    {
        if (predicate != null)
        {
            return context.Set<T>().FirstOrDefaultAsync(predicate);

        }
        return context.Set<T>().FirstOrDefaultAsync();
    }

    public Guid Add(T entity)
    {
        context.Set<T>().Add(entity);
        return entity.Id;
    }

    public T Update(T entity)
    {
       context.Set<T>().Update(entity);
       return entity;
    }

    public void Delete(Guid id)
    {
        var entity = context.Set<T>().Find(id);
        if (entity != null)
        {
            context.Set<T>().Remove(entity);
        }
    }

    public  Task<int> SaveChangesAsync(CancellationToken cancellationToken)
    {
        return context.SaveChangesAsync(cancellationToken);
    }
}

