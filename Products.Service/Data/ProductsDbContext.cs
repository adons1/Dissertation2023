using Microsoft.EntityFrameworkCore;
using Products.Service.Models;

namespace Products.Service.Data;

public class ProductsDbContext : DbContext
{
    public DbSet<Product> Products { get; set; } = null!;
    public ProductsDbContext(DbContextOptions<ProductsDbContext> options)
        : base(options)
    {

    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
    }
}
