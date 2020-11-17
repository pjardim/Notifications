using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Notifications.Core.SeedWork
{
    public interface IRepositoryBase<TEntity> : IDisposable where TEntity : Entity
    {
        // Marks an entity as new
        void Add(TEntity entity);

        // Marks an entity as modified
        void Update(TEntity entity);

        // Marks an entity to be removed
        void Remove(Guid id);

        void Remove(Expression<Func<TEntity, bool>> where);

        // Get an entity by Guid id
        TEntity GetById(Guid id);

        Task<TEntity> GetByIdAsync(Guid id);

        // Get an entity using delegate
        TEntity Get(Expression<Func<TEntity, bool>> where);
        Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> where);

        // Gets all entities of type T
        Task<IEnumerable<TEntity>> GetAllAsync();

        // Gets entities using delegate
        IEnumerable<TEntity> GetMany(Expression<Func<TEntity, bool>> where);

        int SaveChanges();

        IEnumerable<TEntity> GetMany(Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, string includeProperties = "");

        Task<IEnumerable<TEntity>> GetManyAsync(Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, string includeProperties = "");
    }
}