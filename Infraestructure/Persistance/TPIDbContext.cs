using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using TPI.Domain.Entities;

namespace TPI.Infraestructure.Persistance
{
    public class TPIDbContext : DbContext
    {
        public DbSet<Product> Products { get; set; } //agregar tablas 

        public TPIDbContext(DbContextOptions<TPIDbContext> options) : base(options) 
        { 
        }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>().UseTpcMappingStrategy();

            modelBuilder.Entity<Product>();

        }

}
}
