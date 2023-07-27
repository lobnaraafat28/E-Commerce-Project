using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Talabat.Core.Entities.Identity;
using Talabat.Repository.Identity;

namespace Talabat.API.Extensions
{
	public static class IdentityServiceExtensions
	{
		public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddIdentity<AppUser, IdentityRole>(options =>
			{

			}).AddEntityFrameworkStores<AppIdentityDbContext>();
			services.AddAuthentication(options =>
			{
				options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
			}).AddJwtBearer(options =>
			{
				options.TokenValidationParameters = new TokenValidationParameters()
				{
                  ValidateIssuer = true,
				  ValidIssuer = configuration["JWT:Validissuer"],
				  ValidateAudience = true,
				  ValidAudience = configuration["JWT:ValidAudience"],
				   ValidateLifetime = true,
				   ValidateIssuerSigningKey = true,
				   IssuerSigningKey= new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Key"]))
				};
			}
			);
			return services;
		}
	}
}
