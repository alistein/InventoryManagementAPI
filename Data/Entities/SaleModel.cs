using System;
namespace InventoryManagementSystem.Data.Entities
{
	//One
	public class SaleModel: BaseEntity<int>
	{
		public int QuantitySold { get; set; }

        public int ItemId { get; set; }

        public DateTime DateOfSale { get; set; }

		public ItemModel? Item { get; set; }

	}
}

