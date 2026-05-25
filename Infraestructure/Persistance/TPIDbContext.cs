using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace TPI.Infraestructure.Persistance
{
    public class TPIDbContext : DbContext
    {
        //public DbSet<Product> Products (get; set; })
        public TPIDbContext(DbContextOptions<TPIDbContext> options) :base(options) 
        { 
        }
    }
}
