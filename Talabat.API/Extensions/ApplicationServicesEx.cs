using Talabat.API.Helpers;
using Talabat.Core;
using Talabat.Core.Services;
using Talabat.Repository;
using Talabat.Service;

namespace Talabat.API.Extensions
{
	public static class ApplicationServicesEx
	{
		public static IServiceCollection AddApplicationServ(this IServiceCollection services)
		{
			services.AddEndpointsApiExplorer();
			services.AddControllers();
			//the object will be exists until the user close the API Project
			services.AddSingleton<IResponseCacheService, ResponseCacheService>();
			services.AddScoped<IUnitOfWork, UnitOfWork>();
			services.AddScoped<IOrderService, OrderService>();
			services.AddScoped<ITokenService, TokenService>();
			services.AddScoped<IPaymentService, PaymentService>();
			services.AddScoped(typeof(IBasketRepository), typeof(BasketRepository));

			services.AddAutoMapper(typeof(MappingProfiles));
			return services;
		}
	}
}
