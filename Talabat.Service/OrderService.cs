using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Orders;
using Talabat.Core.Repositories;
using Talabat.Core.Services;
using Talabat.Core.Specifications.OrderSpec;
using Talabat.Repository;

namespace Talabat.Service
{
	public class OrderService : IOrderService
	{
		private readonly IBasketRepository _basketRepository;
		private readonly IUnitOfWork _unitOfWork;
		private readonly IPaymentService _paymentService;

		public OrderService(IBasketRepository basketRepository,IUnitOfWork unitOfWork, IPaymentService paymentService)
		{
			_basketRepository = basketRepository;
			_unitOfWork = unitOfWork;
			_paymentService = paymentService;
		}

		public async Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync()
		{

			var deliveryMethods = await _unitOfWork.Repository<DeliveryMethod>().GetAllAsync();
			return deliveryMethods;
		}

		public async Task<Order> GetOrderAsync(string buyerEmail, string basketId, int deliveryMethodId, Address ShipToAddress)
		{
			var deliveryMethod = new DeliveryMethod();
			var basket = await _basketRepository.GetBasketAsync(basketId);
			var Items = new List<OrderItem>();
			if(basket != null){
				foreach (var item in basket.items)
				{
					var productRepository = _unitOfWork.Repository<Product>();
					if(productRepository != null)
					{
						var product = await productRepository.GetByIdAsync(item.Id);
						var productItem = new ProductItemOrder(product.Id, product.Name, product.PictureUrl);
						var orderItem = new OrderItem(productItem, product.Price, item.Quantity);
						Items.Add(orderItem);
					}
					
				}
				
				
			}
			var subtotalPrice = Items.Sum(x => (x.Price) * (x.Quantity));
			var deliveryMethodrepository = _unitOfWork.Repository<DeliveryMethod>();
			if(deliveryMethodrepository != null)
			  deliveryMethod =await deliveryMethodrepository.GetByIdAsync(deliveryMethodId);
			var spec = new OrderWithPaymentIntentIdSpec(basket.PaymentIntentId);
			
			var existingOrder = await _unitOfWork.Repository<Order>().GetByIdSpecAsync(spec);
			if(existingOrder!= null)
			{
				_unitOfWork.Repository<Order>().Delete(existingOrder);
				//update the amount of payment intent
				await _paymentService.CreateOrUpdatePaymentIntent(basket.Id);
			}
			var order = new Order()
			{
				Email = buyerEmail,
				ShipToAddress = ShipToAddress,
				SubTotal = subtotalPrice,
				DeliveryMethod = deliveryMethod,
				Items = Items
			};
			var orderRepository = _unitOfWork.Repository<Order>();
			if (orderRepository != null)
			{
				await orderRepository.Add(order);
				var SaveChanges = await _unitOfWork.Complete();
				if (SaveChanges >= 0) return order;
			}
			return null;
			

		}

		public async Task<Order?> GetOrderByIdOrUserAsync(string buyerEmail, int orderId)
		{

			var spec = new OrderSpecification(buyerEmail,orderId);
			var order = await _unitOfWork.Repository<Order>().GetByIdSpecAsync(spec);
			return order;
		}

		public async Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string buyerEmail)
		{
			var spec = new OrderSpecification(buyerEmail);
			var orders = await _unitOfWork.Repository<Order>().GetAllSpecAsync(spec);
			return orders;
		}
	}
}
