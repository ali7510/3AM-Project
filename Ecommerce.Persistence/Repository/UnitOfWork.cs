using Ecommerce.Domain;
using Ecommerce.Domain.Contracts;
using Ecommerce.Persistence.Data.DBcontexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Persistence.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly StoreDbContext _dbContext;
        private readonly Dictionary<Type, object> _repositories = [];

        public UnitOfWork(StoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity
        {
            var type = typeof(TEntity);
            if(_repositories.TryGetValue(type, out object? repository))
            {
                return (IGenericRepository<TEntity>)_repositories!;
            }
            var newRepository = new GenericRepository<TEntity>(_dbContext);
            _repositories.Add(type, newRepository);
            return newRepository;
        }

        public async Task<int> SaveChanges()
        {
            return await _dbContext.SaveChangesAsync();
        }
    }
}
