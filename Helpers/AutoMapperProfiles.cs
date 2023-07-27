using System;
using AutoMapper;
using InventoryManagementSystem.Data.Entities;
using InventoryManagementSystem.DTOs;

namespace InventoryManagementSystem.Helpers
{
	public class AutoMapperProfiles: Profile
	{
		public AutoMapperProfiles()
		{
			CreateMap<ItemModel, CreateItemDTO>().ReverseMap();

			CreateMap<ItemModel, GetItemDto>().ReverseMap();

			CreateMap<SaleModel, GetSaleDto>().ReverseMap();

			CreateMap<SaleModel, CreateSaleDTO>().ReverseMap();
		}
	}
}

