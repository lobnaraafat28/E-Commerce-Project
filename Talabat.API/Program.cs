using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using Talabat.API.Extensions;
using Talabat.API.Helpers;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Repositories;
using Talabat.Repository;
using Talabat.Repository.Data;
using Talabat.Repository.Identity;

namespace Talabat.API
{
	public class Program
	{
		public static async Task Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.
			builder.Services.AddApplicationServ();
			builder.Services.AddIdentityServices(builder.Configuration);
			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
			builder.Services.AddCors(options =>
			{
				options.AddPolicy("MyPolicy", options =>
				{
					options.AllowAnyHeader().AllowAnyMethod().WithOrigins(builder.Configuration["FrontBaseUrl"]);
				});
			});
			builder.Services.AddSwaggerGen();
			builder.Services.AddDbContext<StoreContext>(options =>
			{
				options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
			});
			builder.Services.AddDbContext<AppIdentityDbContext>(options =>
			{
				options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection"));
			});
			builder.Services.AddSingleton<IConnectionMultiplexer>(options =>
			{
				var connection = ConfigurationOptions.Parse(builder.Configuration.GetConnectionString("Redis"));
				return ConnectionMultiplexer.Connect(connection);
			});
			var app = builder.Build();
			
			var scope = app.Services.CreateScope();
			var services = scope.ServiceProvider;
			var loggerF = services.GetRequiredService<ILoggerFactory>();
			try
			{
				var dbContext = services.GetRequiredService<StoreContext>();
				await dbContext.Database.MigrateAsync();
				await StoreContextSeed.SeedAsync(dbContext);
				var identityContext = services.GetRequiredService<AppIdentityDbContext>();
				await identityContext.Database.MigrateAsync();
				var userManager = services.GetRequiredService<UserManager<AppUser>>();
				await AppIdentityDbContextDataSeed.SeedUsersAsync(userManager);

			}
			catch (Exception ex)
			{

				var logger = loggerF.CreateLogger<Program>();
				logger.LogError(ex, "an error occured during apply migration");
			}

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}
			app.UseStatusCodePagesWithRedirects("/errors/{0}");

			app.UseHttpsRedirection();

			app.UseStaticFiles();
			app.UseCors("MyPolicy");
			app.UseAuthentication();
			app.UseAuthorization();
			app.MapControllers();

			app.Run();
		}
	}
}