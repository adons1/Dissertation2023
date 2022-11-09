using Core;
using CustomersService.TransportTypes.TransportModels;

namespace CustomersService.TransportTypes.TransportServices.Contracts;

public interface ICustomersService
{
    Task<Result<Customer>?> SelectById(Guid guid);
    Task<Result<IEnumerable<Customer>>> SelectAll();
    Task<Result<TokenModel>> Login(LoginCustomer customer);
    Task<Result<bool>> Register(RegisterCustomer customer);
    Task<Result<bool>> Delete(Guid guid);
}
