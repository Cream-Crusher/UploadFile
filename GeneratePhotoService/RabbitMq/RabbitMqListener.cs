using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.Extensions.Hosting;
using System.Text;
using System.Diagnostics;
using System;
using System.Text.Json;
using GeneratePhotoService.Persistence.Repositories;


public class RabbitMqListener : BackgroundService
{
    private readonly IConnection _connection;
    private readonly IModel _channel;
    private readonly IPhotoRepository _photoStorageService;
    private readonly IConfiguration _configuration;
    
    public RabbitMqListener(
        IConfiguration configuration, 
        IPhotoRepository photoRepository
        )
    {
        _photoStorageService = photoRepository;
        
        var localHost = configuration.GetSection("RabbitMq:HostName").Value;
        var myQueue = configuration.GetSection("RabbitMq:QueueName").Value;
        var userName = configuration.GetSection("RabbitMq:UserName").Value;
        var password = configuration.GetSection("RabbitMq:Password").Value;
        
        var factory = new ConnectionFactory
        {
            UserName = userName,
            Password = password,
            HostName = localHost,
            Port = AmqpTcpEndpoint.UseDefaultPort
        };
        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
        _channel.QueueDeclare(queue: myQueue, durable: false, exclusive: false, autoDelete: false, arguments: null);
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        stoppingToken.ThrowIfCancellationRequested();

        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += (ch, ea) =>
        {
            var content = Encoding.UTF8.GetString(ea.Body.ToArray());
            var dictionary = JsonSerializer.Deserialize<Dictionary<string, string>>(content);

            var photoId = new Guid(dictionary["photoId"]);
            var raceStageId = new Guid(dictionary["raceStageId"]);
            var key = new string(dictionary["key"]);
            
            _photoStorageService.UploadGeneratedPhotos(
                photoId,
                raceStageId,
                _configuration.GetSection("Storage:bucketName").Value,
                key,
                CancellationToken.None
            );
                
            _channel.BasicAck(ea.DeliveryTag, false);
        };

        _channel.BasicConsume("GeneratePhotoQueue", false, consumer);

        return Task.CompletedTask;
    }
	
    public override void Dispose()
    {
        _channel.Close();
        _connection.Close();
        base.Dispose();
    }
}