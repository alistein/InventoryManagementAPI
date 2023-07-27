using System;
namespace InventoryManagementSystem.Data.Entities
{
	public class UserModel
	{
		public int ID { get; set; }

		public string? Username { get; set; }

		public string? Password { get; set; }

		public List<string> Roles { get; set; } = new List<string>();

	}
}

