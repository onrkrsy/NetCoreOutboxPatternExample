using ECommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Infrastructure.Persistence
{
    public class AppDbContext : DbContext
    {
        protected IConfiguration Configuration { get; set; }
        public AppDbContext(DbContextOptions dbContextOptions, IConfiguration configuration) : base(dbContextOptions) {

            Configuration = configuration;
            //Database.EnsureCreated();
        }

        public DbSet<Order> Orders { get; set; }
        public DbSet<OutboxMessage> OutboxMessages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }  
    }
}
