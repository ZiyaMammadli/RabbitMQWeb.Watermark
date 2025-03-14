using RabbitMQ.Client;
using RabbitMQWeb.Watermark.Services;

namespace RabbitMQWeb.Watermark.Registrations;

public static class RabbitMQRegistration
{
    public static void AddRabbitMQ(this IServiceCollection services,IConfiguration config)
    {
        services.AddSingleton(sp => new ConnectionFactory() { Uri= new Uri(config.GetConnectionString("RabbitMQ"))});
        services.AddSingleton(typeof(RabbitMQClientService));
    }
}
