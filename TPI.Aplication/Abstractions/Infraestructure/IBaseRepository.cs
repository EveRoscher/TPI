using System;
using System.Collections.Generic;
using System.Text;

namespace TPI.Aplication.Abstractions.Infraestructure
{
    public interface IBaseRepository <T> where T : class
    {
        List<T> GetAll();
        T? GetById(Guid id);
        T Add(T entity);
        void Update(T entity);
        void Delete(Guid id);
    }
}
