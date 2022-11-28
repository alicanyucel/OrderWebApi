using RabbitMQ.Client;
namespace OrderAppWebApi.RabbitMq
{
    public class SetQueue
    {
        public static void SendQueue(byte[] datas)
        {
            var factory = new ConnectionFactory();
            factory.Uri = new Uri("amqps://kvotigyo:9X7sYWek66Z6QYHqBXfsM0K-inkgCZom@chimpanzee.rmq.cloudamqp.com/kvotigyo");

            using var connection = factory.CreateConnection();
            var channel = connection.CreateModel();

            channel.QueueDeclare("sendTask", true, false, false);

            channel.BasicPublish(String.Empty, "sendTask", null, datas);
        }
    }
}
