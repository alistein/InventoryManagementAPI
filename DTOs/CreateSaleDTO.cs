using System;
namespace InventoryManagementSystem.DTOs
{
	public class CreateSaleDTO
	{
        public int QuantitySold { get; set; }

        public int ItemId { get; set; }

        public DateTime DateOfSale { get; set; }
    }
}

