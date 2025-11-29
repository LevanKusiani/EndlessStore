using Confluent.Kafka;
using EndlessStore.Inventory.Core.Application.Services;

namespace EndlessStore.Inventory.Infrastructure.Kafka
{
    public class KafkaService : IKafkaService
    {
        private readonly IProducer<string, string> _producer;
        private readonly ILogger<KafkaService> _logger;

        public KafkaService(IProducer<string, string> producer, ILogger<KafkaService> logger)
        {
            _producer = producer;
            _logger = logger;
        }

        public async Task ProduceAsync(string topic, string key, string value)
        {
            var kafkaMessage = new Message<string, string>
            {
                Key = key ?? Guid.NewGuid().ToString(), // handle this differently if needed
                Value = System.Text.Json.JsonSerializer.Serialize(value)
            };

            var result = await _producer.ProduceAsync(topic, message: kafkaMessage);

            _logger.LogInformation($@"Message sent to:
                Topic: {result.Topic}
                Key: {result.Key}
                Partition: {result.Partition.Value}
                Offset: {result.Offset}
                Timestamp: {result.Timestamp.UtcDateTime}
                ");
        }
    }
}
