using Talabat.Core.Entities.Orders;

namespace Talabat.API.DTO
{
	public class OrderDTO
	{
		public string BasketId { get; set; }
		public int DeliveryMethodId { get; set; }
		public Address ShipToAddress { get; set; }
	}
}
