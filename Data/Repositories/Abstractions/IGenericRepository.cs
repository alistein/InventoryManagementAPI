using System;
using InventoryManagementSystem.Data.Entities;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManagementSystem.Data.Repositories.Abstractions
{
	// TEntity must be reference type that inherited from BaseEntity
	// Tkey is for key type for IDs
	public interface IGenericRepository<TEntity, in TKey> : IDisposable where TEntity: BaseEntity<TKey>
	{
		public Task<IEnumerable<TEntity>> GetAllList();

		public IQueryable<TEntity> GetAllData();

		public ValueTask<TEntity> Find(int id);
		
		public Task<TEntity> GetSingleData(TKey id);

		public Task<TEntity> AddData(TEntity entity);

		public void RemoveData(TEntity entity);

		public void EditData(TEntity entity);

	}
}

