using System;
using System.Collections;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces;

namespace Infrastructure.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly StoreContext context;
        private Hashtable repositories;

        public UnitOfWork(StoreContext context)
        {
            this.context = context;
        }

        public async Task<int> Complete()
        {
            return await context.SaveChangesAsync();
        }

        public void Dispose()
        {
            context.Dispose();
        }

        public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity
        {
            // Create instance hashtable
            if (repositories == null) repositories = new Hashtable();

            // Find repository exists
            // 1. Get Name Type
            var type = typeof(TEntity).Name;

            // 2. Check exists
            if (!repositories.ContainsKey(type))
            {
                var repositoryType = typeof(GenericRepository<>);

                // Create instance repository
                var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(TEntity)), context);

                repositories.Add(type, repositoryInstance);
            }

            return (IGenericRepository<TEntity>)repositories[type];
        }
    }
}