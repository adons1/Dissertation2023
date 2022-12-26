using Microsoft.AspNetCore.Http.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Exceptions
{
    public class RollbackException : Exception
    {
        public Guid RequestId { get; set; }
        public RollbackException(Guid requestId, string message) : base(message)
        {
            RequestId = requestId;
        }
    }
}
