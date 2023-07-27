using System.ComponentModel.DataAnnotations;
using Talabat.Core.Entities;

namespace Talabat.API.DTO
{
	public class CustomerBasketDTO
	{
		[Required]
		public string Id { get; set; }
		public List<BasketItemDTO> items { get; set; } = new List<BasketItemDTO>();
		public string? PaymentIntentId { get; set; }
		public string? ClientSecret { get; set; }
		public int? DeliveryMethodId { get; set; }
		public decimal shippingCost { get; set; }

	}
}
