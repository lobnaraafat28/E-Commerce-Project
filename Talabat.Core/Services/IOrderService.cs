using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Orders;

namespace Talabat.Core.Services
{
	public interface IOrderService
	{
		Task<Order> GetOrderAsync(string buyerEmail, string basketId, int deliveryMethodId, Address ShipToAddress);
		Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string buyerEmail);
		Task<Order?> GetOrderByIdOrUserAsync(string buyerEmail, int orderId);
		Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync();

	}
}
