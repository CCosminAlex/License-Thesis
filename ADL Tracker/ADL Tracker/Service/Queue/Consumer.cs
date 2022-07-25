using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using ADL_Tracker.Entity;
using ADL_Tracker.Repository;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;


namespace ADL_Tracker.Service.Queue
{
    public class Consumer : IHostedService, IDisposable
    {
        private readonly ILogger<Consumer> _logger;
        private Timer _timer;
        private readonly IConnection _connection;
        private IModel _channel;
        private ManualResetEvent _resetEvent = new ManualResetEvent(false);
        private static readonly string _url = "amqps://voarlwcj:7dxq5JahWMnxDsio-woSyn4CZvRy9U0_@cow.rmq2.cloudamqp.com/voarlwcj";
        private ApplicationDbContext dbContext;
        private TestingDaysRepo repository;


        public Consumer()
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>().UseNpgsql("Host=localhost;Database=ADL;Username=postgres;Password=123;Port=5433");

            dbContext = new ApplicationDbContext(optionsBuilder.Options);
            repository = new TestingDaysRepo(dbContext);

            var factory = new ConnectionFactory
            {
                Uri = new Uri(_url)
            };

            _connection = factory.CreateConnection();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            var start = TimeSpan.Zero;
            _timer = new System.Threading.Timer((e) =>
            {
                ConsumeQueue();

            }, null, TimeSpan.Zero,
                TimeSpan.FromSeconds(5));

            return Task.CompletedTask;

        }

        public void ConsumeQueue()
        {

            try
            {
                _channel = _connection.CreateModel();


                var queueName = "days2";
                bool durable = false;
                bool exclusive = false;
                bool autoDelete = true;

                _channel.QueueDeclare(queueName, durable, exclusive, autoDelete, null);

                var consumer = new EventingBasicConsumer(_channel);


                consumer.Received += (model, deliveryEventArgs) =>
                {
                    var body = deliveryEventArgs.Body.ToArray();

                    var message = Encoding.UTF8.GetString(body);
                    System.Diagnostics.Debug.WriteLine("*#############*", message);

                    var data = JsonConvert.DeserializeObject<List<TestingDays>>(message);
                    Day day = new Day { Days = data };
                    repository.saveToDataBase(data);

                // _channel.BasicAck(deliveryEventArgs.DeliveryTag, false);
            };

                // start consuming
                _ = _channel.BasicConsume(consumer, queueName, true);
                // Wait for the reset event and clean up when it triggers
            }
            catch(Exception e)
            {

            }
        }
        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("MessageListener is stopping.");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _channel?.Close();
            _channel = null;
            _connection?.Close();

            _timer?.Dispose();
        }
    }
}

