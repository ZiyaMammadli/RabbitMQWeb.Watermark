
using NuGet.Protocol.Plugins;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using RabbitMQWeb.Watermark.Services;
using System.Drawing;
using System.Text;
using System.Text.Json;

namespace RabbitMQWeb.Watermark.BackgroundServices;

public class ImageWatermarkProcessBackgroundService : BackgroundService
{
    private readonly RabbitMQClientService _rabbitmqClientService;
    private readonly ILogger<ImageWatermarkProcessBackgroundService> _logger;
    private IChannel _channel;
    public ImageWatermarkProcessBackgroundService(RabbitMQClientService rabbitmqClientService, ILogger<ImageWatermarkProcessBackgroundService> logger)
    {
        _rabbitmqClientService = rabbitmqClientService;
        _logger = logger;
    }

    public override async Task StartAsync(CancellationToken cancellationToken)
    {
        _channel = await _rabbitmqClientService.Connect();

        await _channel.BasicQosAsync(0, 1, false);
        await base.StartAsync(cancellationToken);
    }
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var consume = new AsyncEventingBasicConsumer(_channel);

        await _channel.BasicConsumeAsync(RabbitMQClientService.queueName, false, consume);

        consume.ReceivedAsync += Consume_ReceivedAsync;

        await Task.CompletedTask;
    }

    private Task Consume_ReceivedAsync(object sender, BasicDeliverEventArgs @event)
    {
        try
        {
            var productImageCreatedEvent = JsonSerializer.Deserialize<ProductImageCreatedEvent>(Encoding.UTF8.GetString(@event.Body.ToArray()));

            var path = Path.Combine(Directory.GetCurrentDirectory(),"wwwroot\\images",productImageCreatedEvent.ImageName);

            string siteName = "www.ZiyaMammadli.com";

            using Image img = Image.FromFile(path);

            using Graphics graphics = Graphics.FromImage(img);

            Font font = new Font(FontFamily.GenericMonospace, 40, FontStyle.Bold, GraphicsUnit.Pixel);

            SizeF textSize = graphics.MeasureString(siteName, font);

            Color color = Color.White;

            var brush = new SolidBrush(color);

            var position = new Point(img.Width - ((int)textSize.Width + 30), img.Height - ((int)textSize.Height + 30));

            graphics.DrawString(siteName, font, brush, position);

            img.Save("wwwroot/images/watermarks/" + productImageCreatedEvent.ImageName);

            img.Dispose();
            graphics.Dispose();

            _channel.BasicAckAsync(@event.DeliveryTag, false);

        }
        catch (Exception ex)
        {

            _logger.LogError(ex.Message, ex);
        }

        return Task.CompletedTask;
    }

    public override Task StopAsync(CancellationToken cancellationToken)
    {
        return base.StopAsync(cancellationToken);
    }
}
