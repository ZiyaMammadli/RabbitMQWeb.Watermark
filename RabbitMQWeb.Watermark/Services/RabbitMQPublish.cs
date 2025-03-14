using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace RabbitMQWeb.Watermark.Services;

public class RabbitMQPublish
{
    private readonly RabbitMQClientService _rabbitmqClientService;

    public RabbitMQPublish(RabbitMQClientService rabbitmqClientService)
    {
        _rabbitmqClientService = rabbitmqClientService;
    }

    public async Task Puplish(ProductImageCreatedEvent imageWatermarkEvent)
    {
        var imageStr=JsonSerializer.Serialize(imageWatermarkEvent);
        var imageByte=Encoding.UTF8.GetBytes(imageStr);

        var channel=await _rabbitmqClientService.Connect();

        var properties=new BasicProperties();
        properties.Persistent = true;

        await channel.BasicPublishAsync(RabbitMQClientService.exchangeName, RabbitMQClientService.routingKey,true,properties, imageByte);

    }
}
