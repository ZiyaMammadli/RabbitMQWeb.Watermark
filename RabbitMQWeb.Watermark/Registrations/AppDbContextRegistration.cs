using Microsoft.EntityFrameworkCore;
using RabbitMQWeb.Watermark.Contexts;

namespace RabbitMQWeb.Watermark.Registrations;

public static class AppDbContextRegistration
{
    public static void AddAppDbContext(this IServiceCollection services,IConfiguration config)
    {
        services.AddDbContext<AppDbContext>(opt =>
        {
            opt.UseSqlServer(config.GetConnectionString("default"));
        });
    }
}
