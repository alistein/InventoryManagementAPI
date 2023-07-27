using System;
using Microsoft.EntityFrameworkCore;
using InventoryManagementSystem.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InventoryManagementSystem.Data.ConfigurationTables
{
    public class SalesConfiguration : IEntityTypeConfiguration<SaleModel>
    {
        void IEntityTypeConfiguration<SaleModel>.Configure(EntityTypeBuilder<SaleModel> builder)
        {
            //Changing the name of table
            builder.ToTable("SalesTable");

            builder.Property(s => s.QuantitySold).IsRequired().HasMaxLength(10);

            builder.Property(s => s.DateOfSale).HasDefaultValueSql("GetDate()");

            builder.HasOne(s => s.Item).WithMany(i => i.Sales).HasForeignKey(s => s.ItemId);

        }
    }
}

