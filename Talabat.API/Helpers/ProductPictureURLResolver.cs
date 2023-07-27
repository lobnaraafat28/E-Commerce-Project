using AutoMapper;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Talabat.API.DTO;
using Talabat.Core.Entities;

namespace Talabat.API.Helpers
{
	public class ProductPictureURLResolver : IValueResolver<Product, ProductToReturnDTO, string>
	{
		private readonly IConfiguration _configuration;

		public ProductPictureURLResolver(IConfiguration configuration)
		{
			_configuration = configuration;
		}
		public string Resolve(Product source, ProductToReturnDTO destination, string destMember, ResolutionContext context)
		{
			if(!string.IsNullOrEmpty(source.PictureUrl))
			{
				return $"{_configuration["APIBaseURL"]}{source.PictureUrl}";
			}
			return string.Empty;
		}
	}
}
