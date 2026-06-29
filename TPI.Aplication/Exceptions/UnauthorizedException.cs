using System;
using System.Collections.Generic;
using System.Text;

namespace TPI.Aplication.Exceptions
{
    public class UnauthorizedException : Exception
    {
        public UnauthorizedException(string message) : base(message) { }
    }
}
