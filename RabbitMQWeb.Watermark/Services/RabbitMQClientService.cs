using RabbitMQ.Client;

namespace RabbitMQWeb.Watermark.Services;

public class RabbitMQClientService:IDisposable
{
    private readonly ConnectionFactory _connectionFactory;
    private IConnection _connection;
    private IChannel _channel;
    private static string queueName = "queue-image-watermark";
    private static string exchangeName = "ImageDirectExchange";
    private static string routingKey = "route-image-watermark";

    private readonly ILogger<RabbitMQClientService> _logger;

    public RabbitMQClientService(ConnectionFactory connectionFactory, ILogger<RabbitMQClientService> logger)
    {
        _connectionFactory = connectionFactory;
        _logger = logger;
        Connect();
    }

    public async Task<IChannel> Connect()
    {
        _connection=await _connectionFactory.CreateConnectionAsync();
        _channel=await _connection.CreateChannelAsync();

        if (_channel is { IsOpen: true })
        {
            return _channel;
        }

        await _channel.QueueDeclareAsync(queueName,true,false,false);
        await _channel.ExchangeDeclareAsync(exchangeName, ExchangeType.Direct, true, false, null);
        await _channel.QueueBindAsync(queueName, exchangeName, routingKey, null);

        _logger.LogInformation("Elaqe quruldu...");

        return _channel;
    }

    public void Dispose()
    {
        _channel?.CloseAsync();
        _channel?.Dispose();
        _connection?.CloseAsync();
        _connection?.Dispose();
        _logger.LogInformation("connection baglandi...");
    }
}
