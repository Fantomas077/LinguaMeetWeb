using System;
using System.Collections.Generic;
using System.Text;

namespace LinguaMeet.Application.Common.Exceptions
{
    public class NotFoundException:Exception
    {
        public NotFoundException(string message):base(message)
        {
            
        }
    }
}
