using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Options;
using PixHub.Config;
using RabbitMQ.Client;

namespace PixHub.Services;

public class MessageService(IOptions<QueueConfig> queueConfig)
{
  readonly string _hostName = queueConfig.Value.HostName;
  readonly string _userName = queueConfig.Value.UserName;
  readonly string _password = queueConfig.Value.Password;

  public void SendMessage(object obj, string queue)
  {
    ConnectionFactory factory = new()
    {
      HostName = _hostName,
      UserName = _userName,
      Password = _password
    };
    using IConnection connection = factory.CreateConnection();
    using IModel channel = connection.CreateModel();

    channel.QueueDeclare(
      queue: queue,
      durable: true,
      exclusive: false,
      autoDelete: false,
      arguments: null
    );

    string json = JsonSerializer.Serialize(obj);
    var body = Encoding.UTF8.GetBytes(json);

    IBasicProperties properties = channel.CreateBasicProperties();
    properties.Persistent = true;

    channel.BasicPublish(
      exchange: string.Empty,
      routingKey: queue,
      basicProperties: properties,
      body: body
    );
  }
}