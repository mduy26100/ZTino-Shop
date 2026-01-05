using Application.Features.Orders.v1.DTOs;
using Domain.Models.Orders;

namespace Application.Features.Orders.v1.Mappings
{
    public class MappingOrderProfile : Profile
    {
        public MappingOrderProfile()
        {
            CreateMap<ShippingAddressDto, OrderAddress>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.OrderId, opt => opt.Ignore())
                .ForMember(dest => dest.Order, opt => opt.Ignore());
        }
    }
}