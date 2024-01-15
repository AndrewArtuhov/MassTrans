using MassTransit;
using Newtonsoft.Json;
using SharedModels;

namespace Consumer
{
    public class OrderCreatedConsumer : IConsumer<IOrderCreated>
    {
        public async Task Consume(ConsumeContext<IOrderCreated> context)
        {
            var jsonMessage = JsonConvert.SerializeObject(context.Message);
            Console.WriteLine($"OrderCreated message: {jsonMessage}");
        }
    }
}
