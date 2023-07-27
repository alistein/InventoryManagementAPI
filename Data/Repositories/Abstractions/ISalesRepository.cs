using System;
using System.Linq.Expressions;
using InventoryManagementSystem.Data.Entities;

namespace InventoryManagementSystem.Data.Repositories.Abstractions
{
	public interface ISalesRepository<TEntity,TKey>: IGenericRepository<TEntity,TKey> where TEntity : BaseEntity<TKey>
	{
		public Task<IEnumerable<object>> GetReport(Expression<Func<TEntity, IQueryable<object>>> func);
	}
}

