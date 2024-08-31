using ECommerce.Application.Events;
using ECommerce.Application.Interfaces;
using MassTransit;

namespace ECommerce.OrderConsumer.API.Consumers
{
    public class OrderCreatedConsumer : IConsumer<OrderCreatedEvent>
    {
        private readonly ILogger<OrderCreatedConsumer> _logger; 

        public OrderCreatedConsumer(ILogger<OrderCreatedConsumer> logger)
        {
            _logger = logger; 
        }

        public async Task Consume(ConsumeContext<OrderCreatedEvent> context)
        {
            var message = context.Message;
            Console.WriteLine($"Received OrderCreatedEvent: OrderId={message.OrderId}");

            try
            {
                // Burada siparişi işlemek için gerekli business uygulanır 
                Console.WriteLine($"Successfully processed OrderCreatedEvent: OrderId={message.OrderId}");
             
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing OrderCreatedEvent: OrderId={message.OrderId}");
                // Hata durumunda yeniden deneme veya dead-letter queue'ya gönderme işlemleri burada yapılabilir
            }
        }
    }
}
