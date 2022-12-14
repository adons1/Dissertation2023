using CustomersService.Models;
using CustomersService.TransportTypes.TransportModels;
using CustomerTransport = CustomersService.TransportTypes.TransportModels.Customer;

namespace CustomersService.Providers.ProvidersContracts;

public interface ICustomersProvider
{
    CustomerTransport? SelectById(Guid guid);
    CustomerTransport? SelectByEmail(string email);
    bool SelectByEmailAndPassword(string email, string password);
    IEnumerable<CustomerTransport> SelectAll(IEnumerable<Guid>? excludeGuids = null);
    bool Create(CustomerIdentity customer);
    bool Delete(Guid guid);
    bool Update(CustomerTransport customer);
}
