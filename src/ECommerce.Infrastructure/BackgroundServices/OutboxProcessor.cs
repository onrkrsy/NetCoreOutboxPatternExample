using ECommerce.Infrastructure.Persistence;
using Microsoft.Extensions.DependencyInjection;
using ECommerce.Infrastructure.Persistence;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ECommerce.Application.Events;


namespace ECommerce.Infrastructure.BackgroundServices
{
    public class OutboxProcessor : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
 

        public OutboxProcessor(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await ProcessOutboxMessagesAsync();
                await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
            }
        }

        private async Task ProcessOutboxMessagesAsync()
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var publishEndpoint = scope.ServiceProvider.GetRequiredService<IPublishEndpoint>();

            var messages = await dbContext.OutboxMessages
                .Where(m => m.ProcessedAt == null)
                .Take(10)
                .ToListAsync();

            foreach (var message in messages)
            {
                try
                {
                    Type eventType = typeof(OrderCreatedEvent);
        
                    var eventData = System.Text.Json.JsonSerializer.Deserialize(message.Content, eventType);

                    // Belirli bir kuyruğa mesaj gönderme
                    if (eventType == typeof(OrderCreatedEvent))
                    {
                        await publishEndpoint.Publish(eventData, context => {
                            context.SetRoutingKey("OrderCreate");
                        });
                    }
                    else
                    {
                        // Diğer event tipleri için farklı kuyruklar tanımlayabilirsiniz
                        await publishEndpoint.Publish(eventData);
                    }

                    message.ProcessedAt = DateTime.UtcNow;
                    await dbContext.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error processing message {message.Id}: {ex.Message}");
                }
            }
        }
    }
}
