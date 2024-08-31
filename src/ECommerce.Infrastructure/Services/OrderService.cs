using ECommerce.Application.DTOs;
using ECommerce.Application.Events;
using ECommerce.Application.Interfaces;
using ECommerce.Domain.Entities;
using ECommerce.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ECommerce.Infrastructure.Services
{
    public class OrderService : IOrderService
    {
        private readonly AppDbContext _dbContext;

        public OrderService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> CreateOrderAsync(CreateOrderDto orderDto)
        {
            using var transaction = await _dbContext.Database.BeginTransactionAsync();

            try
            {
                var order = new Order
                {
                    CustomerName = orderDto.CustomerName,
                    TotalAmount = orderDto.TotalAmount,
                    CreatedAt = DateTime.UtcNow
                };

                _dbContext.Orders.Add(order);
                await _dbContext.SaveChangesAsync();

                var outboxMessage = new OutboxMessage
                {
                    Id = Guid.NewGuid(),
                    Type = nameof(OrderCreatedEvent),
                    Content = JsonSerializer.Serialize(new OrderCreatedEvent { OrderId = order.Id }),
                    CreatedAt = DateTime.UtcNow
                };

                _dbContext.OutboxMessages.Add(outboxMessage);
                await _dbContext.SaveChangesAsync();

                await transaction.CommitAsync();

                return order.Id;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}
