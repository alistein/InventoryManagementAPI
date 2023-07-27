using System;
using InventoryManagementSystem.Data.Entities;
using InventoryManagementSystem.Data.Repositories.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagementSystem.Data.Repositories.Concretes
{
    public class GenericRepository<TEntity, Tkey> : IGenericRepository<TEntity, Tkey> where TEntity : BaseEntity<Tkey>

    {
        private readonly InventoryDbContext _inventoryDbContext;

        private DbSet<TEntity> Table => _inventoryDbContext.Set<TEntity>();

        public GenericRepository(InventoryDbContext inventoryDbContext) => _inventoryDbContext = inventoryDbContext;

        public async Task<TEntity> AddData(TEntity entity)
        {
            await Table.AddAsync(entity);

            return entity;
        }

        public void Dispose()
        {
            _inventoryDbContext.Dispose();
        }

        public void EditData(TEntity entity)
        {
            _inventoryDbContext.Entry(entity).State = EntityState.Modified;
        }

        public async Task<IEnumerable<TEntity>> GetAllList()
        {
            return await Table.ToListAsync();
        }

        public async Task<TEntity> GetSingleData(Tkey id)
        {
            var selectedData = await Table.FindAsync(id) ?? throw new Exception("Cant find result");

            return selectedData;
          
        }

        public void RemoveData(TEntity entity)
        {
           Table.Remove(entity);
        }

        public IQueryable<TEntity> GetAllData()
        {
            IQueryable<TEntity> query = Table;

            return query;
        }

        public ValueTask<TEntity> Find(int id)
        {
            return Table.FindAsync(id)!;
        }
    }
}

