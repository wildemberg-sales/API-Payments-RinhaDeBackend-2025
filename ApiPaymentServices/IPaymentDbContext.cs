using ApiPaymentServices.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiPaymentServices
{
    internal interface IApiDbContext
    {
    }

    internal sealed class ApiDbContext : DbContext, IApiDbContext
    {
        public ApiDbContext(DbContextOptions<ApiDbContext> options) : base(options)
        {
        }

        //Entities
        public DbSet<Payment> Payments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApiDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
 }
