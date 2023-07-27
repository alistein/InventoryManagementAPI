using System;
namespace InventoryManagementSystem.DTOs
{
	public class GetSaleDto
	{
        public int ID { get; set; }

        public int QuantitySold { get; set; }

        public int ItemId { get; set; }

        public DateTime DateOfSale { get; set; }
    }
}

