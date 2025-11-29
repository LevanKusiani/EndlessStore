namespace EndlessStore.Inventory.Core.Application.Services
{
    public interface IKafkaService
    {
        Task ProduceAsync(string topic, string key, string value);
    }
}
