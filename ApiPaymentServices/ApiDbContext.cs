using ApiPaymentServices.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

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
