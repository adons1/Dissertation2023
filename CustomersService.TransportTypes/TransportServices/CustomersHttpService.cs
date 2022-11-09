using Core;
using CustomersService.TransportTypes.TransportModels;
using CustomersService.TransportTypes.TransportServices.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomersService.TransportTypes.TransportServices;

public class CustomersHttpService : HttpServiceBase, ICustomersService
{
    public CustomersHttpService(string baseUrl)
    {
        _baseUrl = baseUrl;
    }

    public async Task<Result<bool>?> Register(RegisterCustomer customer)
    {
        return await PostAsync<bool>($"/customers/register", body:customer, header:new { aaa = "123", bbb = "456"}, query: new { identity = "12fffrwgw" });
    }

    public Task<Result<bool>> Delete(Guid guid)
    {
        throw new NotImplementedException();
    }

    public async Task<Result<IEnumerable<Customer>>?> SelectAll()
    {
        return await GetAsync<IEnumerable<Customer>>($"/customers/select_all", null);
    }

    public async Task<Result<Customer>?> SelectById(Guid guid)
    {

        return await GetAsync<Customer?>($"/customers/select_by_id", new { id = guid });
    }

    public Task<Result<TokenModel>> Login(LoginCustomer customer)
    {
        throw new NotImplementedException();
    }
}
