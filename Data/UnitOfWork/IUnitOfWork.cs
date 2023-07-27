using System;
using InventoryManagementSystem.Data.Entities;
using InventoryManagementSystem.Data.Repositories.Abstractions;

namespace InventoryManagementSystem.Data.UnitOfWork
{
	public interface IUnitOfWork
	{
		public IGenericRepository<ItemModel, int> ItemRepository {get;set;}

		public ISalesRepository<SaleModel, int> SaleRepository { get; set; }

		public Task Commit();
	}
}

