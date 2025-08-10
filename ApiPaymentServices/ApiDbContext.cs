using ApiPaymentServices.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiPaymentServices
{
    public sealed class ApiDbContext : DbContext
    {
        private IConfiguration _configuration;
        
        //Entities
        public DbSet<Payment> Payments { get; set; }

        public ApiDbContext(IConfiguration configuration, DbContextOptions options) : base(options)
        {
            _configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = _configuration.GetConnectionString("Local");
            optionsBuilder.UseNpgsql(connectionString);
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApiDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
 }
