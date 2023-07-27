
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using Talabat.API.DTO;
using Talabat.API.Errors;
using Talabat.API.Helpers;
using Talabat.Core.Entities.Orders;
using Talabat.Core.Services;

namespace Talabat.API.Controllers
{

	public class OrdersController : BaseAPIController
	{
		private readonly IOrderService _orderService;
		private readonly IMapper _mapper;

		public OrdersController(IOrderService orderService, IMapper mapper)
		{
			_orderService = orderService;
			_mapper = mapper;
		}
		[ProducesResponseType(typeof(Order), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
		[HttpPost]
		public async Task<ActionResult<OrderToReturnDTO>> CreateOrder(OrderDTO orderDto)
		{
			var buyerEmail = User.FindFirstValue(ClaimTypes.Email);
			var ShipAddress = _mapper.Map<Address>(orderDto.ShipToAddress);
			await _orderService.GetOrderAsync(buyerEmail, orderDto.BasketId, orderDto.DeliveryMethodId, orderDto.ShipToAddress);
			var order = await _orderService.GetOrderAsync(buyerEmail, orderDto.BasketId, orderDto.DeliveryMethodId, ShipAddress);
			if (order == null) return BadRequest(new ApiResponse(400));
			return Ok(_mapper.Map<Order,OrderToReturnDTO>(order));

		}
		[CashedAttribute(600)]
		[HttpGet]
		public async Task<ActionResult<IReadOnlyList<OrderToReturnDTO>>> GetOrdersForUser()
		{
			var buyerEmail = User.FindFirstValue(ClaimTypes.Email);
			var orders = await _orderService.GetOrdersForUserAsync(buyerEmail);
			return Ok(_mapper.Map < IReadOnlyList< Order>, IReadOnlyList<OrderToReturnDTO>> (orders)); ;
		}
		[ProducesResponseType(typeof(Order), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
		[CashedAttribute(600)]
		[HttpGet("{id}")]
		public async Task<ActionResult<OrderToReturnDTO>> GetOrderForUser(int id)
		{
			var email = User.FindFirstValue(ClaimTypes.Email);
			var order = await _orderService.GetOrderByIdOrUserAsync(email,id);
			if (order == null) return NotFound(new ApiResponse(404));
			return Ok(_mapper.Map<Order,OrderToReturnDTO>(order));
		}
		[CashedAttribute(600)]
		[HttpGet("deliveryMethods")]
		public async Task<ActionResult<IReadOnlyList<DeliveryMethod>>> Getdeliverymethods()
		{
			var deliverymethod = await _orderService.GetDeliveryMethodsAsync();
			return Ok(deliverymethod);
		}

	}
}
