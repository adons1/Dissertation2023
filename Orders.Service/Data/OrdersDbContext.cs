using Microsoft.EntityFrameworkCore;
using Orders.Service.Models;

namespace Orders.Service.Data;

public class OrdersDbContext : DbContext
{
    public DbSet<Order> Orders { get; set; } = null!;
    public OrdersDbContext(DbContextOptions<OrdersDbContext> options)
        : base(options)
    {

    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
    }
}
