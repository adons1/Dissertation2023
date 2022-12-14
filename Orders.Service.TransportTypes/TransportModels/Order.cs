using CustomersService.TransportTypes.TransportModels;
using Products.TransportTypes.TransportModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orders.TransportTypes.TransportModels;

public class Order
{
    public Guid Id { get; set; }
    public Customer Customer { get; set; }
    public IEnumerable<OrderedProduct> Products { get; set; }
}
