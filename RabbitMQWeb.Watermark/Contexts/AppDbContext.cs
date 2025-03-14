using Microsoft.EntityFrameworkCore;
using RabbitMQWeb.Watermark.Models;

namespace RabbitMQWeb.Watermark.Contexts;

public class AppDbContext:DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options):base(options)
    {
        
    }

    public DbSet<Product> Products { get; set; }
}
