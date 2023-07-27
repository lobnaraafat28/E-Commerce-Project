﻿using AutoMapper;
using Talabat.API.DTO;
using Talabat.Core.Entities.Orders;

namespace Talabat.API.Helpers
{
	public class OrderItemPictureUrlResolver : IValueResolver<OrderItem, OrderItemDTO, string>
	{
		private readonly IConfiguration _configuration;

		public OrderItemPictureUrlResolver(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		public string Resolve(OrderItem source, OrderItemDTO destination, string destMember, ResolutionContext context)
		{
			if (!string.IsNullOrEmpty(source.Product.PictureUrl))
			{
				return $"{_configuration["APIBaseURL"]}{source.Product.PictureUrl}";
			}
			return string.Empty;
		}
	}
}
