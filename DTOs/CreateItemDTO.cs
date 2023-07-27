using System;
using System.ComponentModel.DataAnnotations;
using InventoryManagementSystem.Data.Entities;

namespace InventoryManagementSystem.DTOs
{
	public class CreateItemDTO
	{
        [Required]
        [MaxLength(20)]
        public string? Name { get; set; }

        [Required]
        [MaxLength(100)]
        public string? Description { get; set; }

        [Required]
        [Range(1500, 8000)]
        public double Price { get; set; }

        [Required]
        [Range(100, 600)]
        public int Quantity { get; set; }
    }
}

