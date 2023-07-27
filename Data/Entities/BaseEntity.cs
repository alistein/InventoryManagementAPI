using System;
using System.ComponentModel.DataAnnotations;

namespace InventoryManagementSystem.Data.Entities
{
	public abstract class BaseEntity<TPrimaryKey>
	{
		[Key]
		public virtual required TPrimaryKey ID { get; set; }
	}
}

