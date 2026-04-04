using System;
using System.Collections.Generic;
using System.Text;
using TPI.Domain.Entities;

namespace TPI.Aplication.Abstractions
{
    public interface IEmailService //ver no se acepta nulls
    {
        List<Email> GetAll();
        Email GetById(Guid id);
        Email create (Email email);
        Email update (Email email);
        Email delete (Email email);
      
    }
}
