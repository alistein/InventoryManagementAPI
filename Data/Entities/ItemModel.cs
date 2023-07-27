using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventoryManagementSystem.Data.Entities
{
	//Many
	[Table("ItemsTable")]
	public class ItemModel: BaseEntity<int>
	{
		[Required]
		[MaxLength(20)]
		public string? Name { get; set; }

		[Required]
		[MaxLength(100)]
		public string? Description { get; set; }

		[Required]
		public double Price { get; set; }

		[Required]
		public int Quantity { get; set; }

		public ICollection<SaleModel>? Sales { get; set; }

	}
}

