using Microsoft.EntityFrameworkCore;
using Demo.Models;

namespace Demo.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Product> Products { get; set; }
    public DbSet<Demo.Models.User> Users { get; set; }
}
