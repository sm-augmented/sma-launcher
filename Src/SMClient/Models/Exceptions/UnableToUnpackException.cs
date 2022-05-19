using SMClient.Models.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Exceptions
{
    public class UnableToUnpackException : SMException
    {
        public UnableToUnpackException()
        {
        }

        public UnableToUnpackException(string message)
          : base(message)
        {
        }

        public UnableToUnpackException(string message, Exception inner)
          : base(message, inner)
        {
        }
    }
}
