using EndlessStore.Inventory.Core.Application.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace EndlessStore.Inventory.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly IKafkaService _kafkaService;

        public TestController(IKafkaService kafkaService)
        {
            _kafkaService = kafkaService;
        }

        [HttpPost(Name = "Test-message")]
        public async Task<IActionResult> PublishMessage([FromBody] TestMessage message)
        {
            //for (int i = 0; i < 3; i++)
            //{
            //    for (int j = 0; j < 10; j++)
            //    {
            //        var key = $"{message.Key}-{j}";
            //        var value = $"{message.Value}-{j}";
            //        await _kafkaService.ProduceAsync(message.Topic, key, value);
            //    }

            //    Thread.Sleep(1000);
            //}

            await _kafkaService.ProduceAsync(message.Topic, message.Key, message.Value);

            return Ok();
        }
    }

    public class TestMessage
    {
        public string Topic { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
