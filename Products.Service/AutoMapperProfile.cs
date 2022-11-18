using AutoMapper;
using Products.Service.Models;
using System;
using ProductTransport = Products.TransportTypes.TransportModels.Product;

namespace Products.Service;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<Product, ProductTransport>();
    }
}
