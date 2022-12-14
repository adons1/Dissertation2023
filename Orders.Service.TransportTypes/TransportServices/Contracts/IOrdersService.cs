using Core;
using Orders.TransportTypes.TransportModels;
using Products.TransportTypes.TransportModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orders.Service.TransportTypes.TransportServices.Contracts;

public interface IOrdersService
{
    Task<Result<IEnumerable<Order>>> SelectAll();
    Task<Result<Order>> SelectById(Guid id);
    Task<Result<Order>> Create(IEnumerable<Guid> ids);
    Task<Result<bool>> Cancel(Guid id);
}
