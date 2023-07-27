using Microsoft.Extensions.Configuration;
using StackExchange.Redis;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Orders;
using Talabat.Core.Services;
using Talabat.Core.Specifications.OrderSpec;
using Talabat.Repository;
using Product = Talabat.Core.Entities.Product;

namespace Talabat.Service
{
	public class PaymentService : IPaymentService
	{
		private readonly IConfiguration _configuration;
		private readonly IBasketRepository _basketRepository;
		private readonly IUnitOfWork _unitOfWork;

		public PaymentService(IConfiguration configuration, IBasketRepository basketRepository, IUnitOfWork unitOfWork)
		{
			_configuration = configuration;
			_basketRepository = basketRepository;
			_unitOfWork = unitOfWork;
		}
		public async Task<CustomerBasket> CreateOrUpdatePaymentIntent(string basketId)
		{
			//get the configuration of stripe secret key from appsettings
			StripeConfiguration.ApiKey = _configuration["StripeSettings:SecretKey"];
			var basket = await _basketRepository.GetBasketAsync(basketId);
			if (basket == null) return null;
			var shippingPrice = 0m;
			//if the user choose a delivery method calculate the price
			if (basket.DeliveryMethodId.HasValue)
			{
				var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(basket.DeliveryMethodId.Value);
				basket.shippingCost = deliveryMethod.Cost;
				shippingPrice = deliveryMethod.Cost;
			}
			//check the price of every item in the basket
			if(basket?.items?.Count > 0)
			{
				foreach (var item in basket.items)
				{
					var product = await _unitOfWork.Repository<Product>().GetByIdAsync(item.Id);
					if(item.Price!= product.Price)
						item.Price = product.Price;
					
				}
			}
			var service = new PaymentIntentService();
			PaymentIntent paymentIntent;
			//if paymentintent in the basket is null then we create a new one .. if not then we update it
			if (string.IsNullOrEmpty(basket.PaymentIntentId))  //create
			{
				var options = new PaymentIntentCreateOptions()
				{
					//we multiply by 100 because the amount is in cent 
					Amount = (long)(basket.items.Sum(item => item.Price * item.Quantity) * 100),
					Currency = "usd",
					PaymentMethodTypes = new List<string>() { "card" }
				};
				paymentIntent = await service.CreateAsync(options);
				basket.PaymentIntentId = paymentIntent.Id;
				basket.ClientSecret = paymentIntent.ClientSecret;
			}
			else   //update
			{
				var options = new PaymentIntentUpdateOptions()
				{
					Amount = (long)(basket.items.Sum(item => item.Price * item.Quantity) * 100)
				};
				await service.UpdateAsync(basket.PaymentIntentId, options);
			}
			await _basketRepository.UpdateBasketAsync(basket);
			return basket;

		}

		public async Task<Core.Entities.Orders.Order> UpdatePaymentIntentToSucceedOrFailed(string paymenIntentId, bool isSucceeded)
		{
			var spec = new OrderWithPaymentIntentIdSpec(paymenIntentId);
			var order = await _unitOfWork.Repository<Core.Entities.Orders.Order>().GetByIdSpecAsync(spec);
			if (isSucceeded)
				order.Status = OrderStates.PaymentReceived;
			else
				order.Status = OrderStates.PaymentFailed;
			_unitOfWork.Repository<Core.Entities.Orders.Order>().Update(order);
			await _unitOfWork.Complete();
			return order;
		}
	}
}
