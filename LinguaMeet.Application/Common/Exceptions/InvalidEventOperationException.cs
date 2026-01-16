using System;
using System.Collections.Generic;
using System.Text;

namespace LinguaMeet.Application.Common.Exceptions
{
    public class InvalidEventOperationException:Exception
    {
        public InvalidEventOperationException(string message):base(message)
        {
            
        }
    }
}
