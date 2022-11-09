using CustomersService.Providers.ProvidersContracts;
using CustomersService.TransportTypes.TransportServices.Contracts;
using CustomersService.TransportTypes.TransportModels;
using Core.Cryptography;
using Microsoft.Extensions.Caching.Distributed;
using Core.RedisKeys;
using Newtonsoft.Json;
using System;
using System.Runtime.InteropServices;
using Microsoft.AspNetCore.Components.Forms;
using CustomersService.Models;
using Customer = CustomersService.TransportTypes.TransportModels.Customer;
using Core;
using CustomersService.Providers;

namespace CustomersService.Services;

public class CustomersService : ICustomersService
{
    readonly ICustomersProvider _customersProvider;
    readonly IDistributedCache _distributedCache;
    public CustomersService(ICustomersProvider customersProvider, IDistributedCache distributedCache)
    {
        _customersProvider = customersProvider;
        _distributedCache = distributedCache;
    }

    public async Task<Result<bool>> Register(RegisterCustomer customer)
    {
        var existingCustomer = _customersProvider.SelectByEmail(customer.Email);

        if (existingCustomer != null)
            return await Task.FromResult(new SuccessResult<bool>(false));

        var customerIdentity = new CustomerIdentity
        {
            Customer = new Models.Customer
            {
                Id = Guid.NewGuid(),
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Birthday = customer.Birthday,
                Account = customer.Account
            },
            Email = customer.Email,
            Password = SHA256.Hash(customer.Password)
        };

        _customersProvider.Create(customerIdentity);

        return await Task.FromResult(new SuccessResult<bool>(true));
    }

    public async Task<Result<bool>> Delete(Guid guid) => new SuccessResult<bool>(await Task.FromResult(_customersProvider.Delete(guid)));

    public async Task<Result<IEnumerable<Customer>>> SelectAll()
    {
        var customers = new SuccessResult<IEnumerable<Customer>>(_customersProvider.SelectAll());
        return await Task.FromResult(customers);
    }

    public async Task<Result<Customer>?> SelectById(Guid guid)
    {
        var customerCachedJson = await _distributedCache.GetStringAsync(ChacheKeys.Customer(guid));

        if (customerCachedJson != null) return await Task.FromResult(new SuccessResult<Customer>(JsonConvert.DeserializeObject<Customer>(customerCachedJson)));

        var customer = _customersProvider.SelectById(guid);

        if (customer == null) return null;

        await _distributedCache.SetStringAsync(ChacheKeys.Customer(guid), JsonConvert.SerializeObject(customer));

        return await Task.FromResult(new SuccessResult<Customer>(customer));
    }

    public async Task<Result<TokenModel>> Login(LoginCustomer customer)
    {
        var isCustomerExists = _customersProvider.SelectByEmailAndPassword(customer.Email, customer.Password);

        if (!isCustomerExists)
            return await Task.FromResult(new FailureResult<TokenModel>(null));

        var randomizer = new Randomizer();
        var token = randomizer.RandomString(100, true);

        var customerModel = _customersProvider.SelectByEmail(customer.Email);

        var tokenModel = new TokenModel
        {
            ClientId = customerModel.Id,
            IssueDate = DateTime.UtcNow,
            Token = token,
        };

        await _distributedCache.SetStringAsync(ChacheKeys.ClientToken(tokenModel.Token), JsonConvert.SerializeObject(tokenModel));

        return await Task.FromResult(new SuccessResult<TokenModel>(tokenModel));
    }

}
