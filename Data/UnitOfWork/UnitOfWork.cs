using System;
using InventoryManagementSystem.Data.Entities;
using InventoryManagementSystem.Data.Repositories.Abstractions;
using InventoryManagementSystem.Data.Repositories.Concretes;

namespace InventoryManagementSystem.Data.UnitOfWork
{
	public class UnitOfWork: IUnitOfWork
	{
        private readonly InventoryDbContext _inventoryDbContext;

        public IGenericRepository<ItemModel, int> ItemRepository { get; set; }

        public ISalesRepository<SaleModel, int> SaleRepository { get; set; }

        public UnitOfWork(InventoryDbContext inventoryDbContext)
		{
            _inventoryDbContext = inventoryDbContext;

            ItemRepository = new GenericRepository<ItemModel,int>(_inventoryDbContext);

            SaleRepository = new SalesRepository<SaleModel, int>(_inventoryDbContext);
		}

        public async Task Commit()
        {
            await _inventoryDbContext.SaveChangesAsync();
        }
    }
}

