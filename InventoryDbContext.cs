using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System;
namespace Assignment1.Models;

public class InventoryDbContext : DbContext
{
    public DbSet<Products> Products { get; set; }
    public DbSet<Categories> Categories { get; set; }
    public DbSet<Orders> Orders { get; set; }
    public DbSet<OrderItems> OrderItems { get; set; }
    public InventoryDbContext(DbContextOptions<InventoryDbContext> options) : base(options) { }
}