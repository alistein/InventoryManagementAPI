using System;
using Microsoft.EntityFrameworkCore;
using InventoryManagementSystem.Data.Entities;
using System.Reflection;
using System.Reflection.Metadata;

namespace InventoryManagementSystem.Data;

public class InventoryDbContext: DbContext
{
    public InventoryDbContext(DbContextOptions<InventoryDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        modelBuilder.Entity<SaleModel>()
        .ToTable(tb => tb.HasTrigger("substract_quantity_after_insert_sale"));

    }


}


