using Core;
using Customers.TransportTypes.TransportModels;
using CustomersService.TransportTypes.TransportModels;
using OauthAuthorization.TransportTypes.TransportModels;
using Products.TransportTypes.TransportModels;

namespace CustomersService.TransportTypes.TransportServices.Contracts;

public interface ICustomersService
{
    Task<Result<Customer>?> SelectById(Guid guid);
    Task<Result<IEnumerable<Customer>>> SelectAll();
    Task<Result<ClientTokenModel>> Login(LoginCustomer customer);
    Task<Result<bool>> Register(RegisterCustomer customer);
    Task<Result<bool>> Delete(Guid guid);
    Task<Result<bool>> Waste(IEnumerable<Guid> productIds, double sum);
    Task<Result<bool>> Earn(double sum);
}
