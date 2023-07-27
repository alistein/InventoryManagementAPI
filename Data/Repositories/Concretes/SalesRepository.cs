using System;
using System.Linq.Expressions;
using InventoryManagementSystem.Data.Entities;
using InventoryManagementSystem.Data.Repositories.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagementSystem.Data.Repositories.Concretes
{
    public class SalesRepository<TEntity, TKey> :
        GenericRepository<TEntity, TKey>, ISalesRepository<TEntity,TKey> where TEntity: BaseEntity<TKey> 
    {
        public readonly InventoryDbContext _inventoryDbContext;

        private DbSet<TEntity> Table => _inventoryDbContext.Set<TEntity>();

        public SalesRepository(InventoryDbContext inventoryDbContext) : base(inventoryDbContext)
        {
            _inventoryDbContext = inventoryDbContext;
        }

        public async Task<IEnumerable<object>> GetReport(Expression<Func<TEntity, IQueryable<object>>> func)
        {
            var reports = await Table.Select(func).ToListAsync();

            return reports;
        }
    }
}

