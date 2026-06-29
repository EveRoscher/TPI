using System;
using System.Collections.Generic;
using System.Text;
using TPI.Domain.Entities;

namespace TPI.Aplication.Abstractions.Infraestructure
{
    public interface IProductRepository : IBaseRepository<Product>
    {
        //object GetAll();
    }
}
