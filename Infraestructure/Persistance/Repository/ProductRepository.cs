using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using TPI.Aplication.Abstractions.Infraestructure;
using TPI.Domain.Entities;

namespace TPI.Infraestructure.Persistance.Repository
{
    public class ProductRepository : BaseRepository<Product>, IProductRepository
    {
        public ProductRepository(TPIDbContext context) : base(context)
        {
        }

        public override List<Product> GetAll()
        {
            return _context.Products
                .Where(p => !p.IsDeleted)
                .ToList();
        }

        public override Product? GetById(Guid id)
        {
            return _context.Products
                .FirstOrDefault(p => p.Id == id && !p.IsDeleted);
        }

       
    }
}
