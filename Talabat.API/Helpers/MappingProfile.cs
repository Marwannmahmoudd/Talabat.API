using AutoMapper;
using Talabat.API.Dtos;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Entities.Order_Aggregate;
using OrderAddress = Talabat.Core.Entities.Order_Aggregate.Address;
using IdentityAddress = Talabat.Core.Entities.Identity.Address;
namespace Talabat.API.Helpers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {

            CreateMap<Product, ProductToReturnDto>()
                .ForMember(p => p.Brand, O => O.MapFrom(s => s.Brand.Name))
                .ForMember(p => p.Category, O => O.MapFrom(s => s.Category.Name))
                    .ForMember(p => p.PictureUrl, O => O.MapFrom<ProductPictrueUrlResolvoer>());



            CreateMap<CustomerBasketDto, CustomerBasket>().ReverseMap();
            CreateMap<BasketItemDto, BasketItem>().ReverseMap();
            CreateMap<AddressDto, OrderAddress>();
            CreateMap<IdentityAddress, AddressDto>().ReverseMap();
           
            //order and ordertoreturndto
            CreateMap<Order, OrderTorReturnDto>()
           .ForMember(o => o.DeliveryMethod, O => O.MapFrom(o => o.DeliveryMethod.ShortName))
           .ForMember(o => o.DeliveryMethodCost, O => O.MapFrom(o => o.DeliveryMethod.Cost));

            CreateMap<OrderItem,OrderItemDto>()
                .ForMember(o => o.ProductName,s => s.MapFrom(o => o.Product.ProductName))
                .ForMember(o => o.ProductId, s => s.MapFrom(o => o.Product.ProductId))
                .ForMember(o => o.PictureUrl, s => s.MapFrom(o => o.Product.PictureUrl))
            .ForMember(o => o.PictureUrl, s => s.MapFrom<OrderPictureUrlReslover>());
        }
    }
}
