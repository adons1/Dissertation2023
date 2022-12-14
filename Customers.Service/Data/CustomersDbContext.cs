using CustomersService.Models;
using Microsoft.EntityFrameworkCore;

namespace CustomersService.Data;

public class CustomersDbContext : DbContext
{
    public DbSet<Customer> Customers { get; set; } = null!;
    public DbSet<CustomerIdentity> CustomerIdentities { get; set; } = null!;
    public CustomersDbContext(DbContextOptions<CustomersDbContext> options)
        : base(options)
    {
        
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
    }
}
