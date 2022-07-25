using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Producer
{
    class Producer
    {
        private static readonly string _url = "amqps://voarlwcj:7dxq5JahWMnxDsio-woSyn4CZvRy9U0_@cow.rmq2.cloudamqp.com/voarlwcj";
        private List<Day> data;

        public Producer(List<Day> data)
        {
            this.data = data;
        }

        public void Publish()
        {
            var factory = new ConnectionFactory
            {
                Uri = new Uri(_url)
            };

            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    var queueName = "days2";
                    bool durable = false;
                    bool exclusive = false;
                    bool autoDelete = true;
                    
                  
                    string message = new string("");
                    channel.QueueDeclare(queueName, durable, exclusive, autoDelete, null);
                
                    var exchangeName = "";
                    var routingKey = queueName;
                    channel.BasicPublish(exchangeName, routingKey, null, Encoding.UTF8.GetBytes(Prepare(data)));

                    Console.WriteLine("OKKK");
              
                }
            }
            


        }

        public static string Prepare(List<Day> data)
        {
            var day = data[0];
            data.RemoveAt(0);
            string jsonSensorData = JsonConvert.SerializeObject(day.DayDetails);
            return jsonSensorData;
        }

    }
}
