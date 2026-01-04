using Application.Features.Carts.v1.DTOs;
using Domain.Models.Carts;

namespace Application.Features.Carts.v1.Mappings
{
    public class MappingCartProfile : Profile
    {
        public MappingCartProfile()
        {
            CreateMap<Cart, UpsertCartResponseDto>()
                .ForMember(dest => dest.CartId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.ProductVariantId, opt => opt.Ignore())
                .ForMember(dest => dest.Quantity, opt => opt.Ignore())
                .ForMember(dest => dest.Message, opt => opt.Ignore());

            CreateMap<UpsertCartDto, CartItem>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CartId, opt => opt.Ignore())
                .ForMember(dest => dest.AddedAt, opt => opt.Ignore())
                .ForMember(dest => dest.Cart, opt => opt.Ignore())
                .ForMember(dest => dest.ProductVariant, opt => opt.Ignore());
        }
    }
}
