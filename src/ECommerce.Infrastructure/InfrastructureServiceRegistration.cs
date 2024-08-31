using ECommerce.Application.Interfaces;
using ECommerce.Infrastructure.BackgroundServices;
using ECommerce.Infrastructure.Persistence;
using ECommerce.Infrastructure.Services;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ECommerce.Infrastructure;

public static class InfrastructureServiceRegistration
{
  

    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services,IHostApplicationBuilder builder)
    {
        //services.AddDbContext<AppDbContext>(options => options.UseInMemoryDatabase("ECommerceOutboxDb"));
        services.AddDbContext<AppDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
        // MassTransit-RabbitMQ Configuration
        services.AddMassTransit(x =>
        {
            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(builder.Configuration["RabbitMQ:Host"], "/", h =>
                {
                    h.Username(builder.Configuration["RabbitMQ:Username"]);
                    h.Password(builder.Configuration["RabbitMQ:Password"]);
                });
            });
        });

        services.AddScoped<IOrderService, OrderService>();
        // Background Services
        services.AddHostedService<OutboxProcessor>();
       
        return services;
    }
}
