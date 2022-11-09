using CustomersService.Data;
using CustomersService.Models;
using CustomersService.Providers.ProvidersContracts;
using Microsoft.EntityFrameworkCore;
using CustomersService.TransportTypes.TransportModels;
using CustomerTransport = CustomersService.TransportTypes.TransportModels.Customer;
using System.Linq.Expressions;
using Core.Cryptography;

namespace CustomersService.Providers;

public class CustomersProvider : ICustomersProvider
{
    readonly CustomersDbContext _customersContext;
    public CustomersProvider(CustomersDbContext customersContext)
    {
        _customersContext = customersContext;
    }

    public CustomerTransport? SelectById(Guid guid)
    {
        return SelectByParameter(c => c.Customer.Id == guid);
    }

    public CustomerTransport? SelectByEmail(string email)
    {
        return SelectByParameter(c => c.Email == email);
    }

    public bool SelectByEmailAndPassword(string email, string password)
    {
        var customer = _customersContext.CustomerIdentities.Include(x => x.Customer)
                    .SingleOrDefault(c => c.Email == email && c.Password == SHA256.Hash(password));

        return customer != null;
    }

    public IEnumerable<CustomerTransport> SelectAll(IEnumerable<Guid>? excludeGuids = null)
    {
        return from customer in _customersContext.CustomerIdentities.Include(x => x.Customer)
               where (excludeGuids ?? new List<Guid>()).All(guid => guid != customer.Customer.Id)
               select
                    new CustomerTransport(customer.Customer.Id, customer.Customer.FirstName, customer.Customer.LastName,
                    customer.Email, customer.Customer.Birthday, customer.Customer.Account);
    }

    public bool Create(CustomerIdentity customer)
    {
        try
        {
            _customersContext.CustomerIdentities.Add(customer);

            _customersContext.SaveChanges();

            return true;
        }
        catch (Exception ex)
        {
            return false;
        }
    }
    public bool Delete(Guid guid)
    {
        var customer = _customersContext.CustomerIdentities
            .Include(x => x.Customer)
            .SingleOrDefault(c => c.Customer.Id == guid);

        if (customer == null) return false;

        _customersContext.CustomerIdentities.Remove(customer);

        return true;
    }

    private CustomerTransport? SelectByParameter(Expression<Func<CustomerIdentity, bool>> expression)
    {
        var customer = _customersContext.CustomerIdentities.Include(x => x.Customer)
            .SingleOrDefault(expression.Compile());

        return
            customer != null
            ?
            new CustomerTransport(customer.Customer.Id, customer.Customer.FirstName, customer.Customer.LastName,
            customer.Email, customer.Customer.Birthday, customer.Customer.Account)
            :
            null;
    }
}
