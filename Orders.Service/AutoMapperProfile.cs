using AutoMapper;
using Orders.Service.Models;
using Products.TransportTypes.TransportModels;
using System;
using OrderTransport = Orders.TransportTypes.TransportModels.Order;
using OrderedProduct = Orders.TransportTypes.TransportModels.OrderedProduct;

namespace Orders.Service;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<Order, OrderTransport>();
        CreateMap<Product, OrderedProduct>();
    }
}
