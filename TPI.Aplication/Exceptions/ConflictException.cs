using System;
using System.Collections.Generic;
using System.Text;

namespace TPI.Aplication.Exceptions
{
    public class ConflictException : Exception
    {
        public ConflictException(string message) : base(message) { }
    }
}
