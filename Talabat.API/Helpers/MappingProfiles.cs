using AutoMapper;
using Talabat.API.DTO;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Entities.Orders;

namespace Talabat.API.Helpers
{
	public class MappingProfiles:Profile
	{
		public MappingProfiles()
		{
			CreateMap<Product,ProductToReturnDTO>()
				.ForMember(d => d.ProductBrand,o => o.MapFrom(d => d.ProductBrand.Name))
		        .ForMember(d => d.ProductType, o => o.MapFrom(d => d.ProductType.Name))
				.ForMember(d => d.PictureUrl,o => o.MapFrom<ProductPictureURLResolver>());
			CreateMap<Core.Entities.Identity.Address, AddressDTO>().ReverseMap();
			CreateMap<CustomerBasketDTO,CustomerBasket>();
			CreateMap<BasketItemDTO, BasketItem>();
			CreateMap<AddressDTO, Core.Entities.Orders.Address>();
			CreateMap<Order, OrderToReturnDTO>().ForMember(d =>d.DeliveryMethod, O=>O.MapFrom(s => s.DeliveryMethod.ShortName)).ForMember(d => d.DeliveryMethodCost, O=>O.MapFrom(s => s.DeliveryMethod.Cost));
			CreateMap<OrderItem, OrderItemDTO>().ForMember(d => d.ProductId, O => O.MapFrom(s => s.Product.ProductId))
				.ForMember(d => d.ProductName, O => O.MapFrom(s => s.Product.ProductName)).ForMember(d => d.PictureUrl, O => O.MapFrom(s => s.Product.PictureUrl))
				.ForMember(d => d.PictureUrl, O =>O.MapFrom<OrderItemPictureUrlResolver>());
		}
	}
}
