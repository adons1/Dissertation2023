using CustomersService.TransportTypes.TransportModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Customers.TransportTypes.TransportServices.Contracts;

public interface IObtainCustomerIdentity
{
    Task<Guid> Identity();
}
