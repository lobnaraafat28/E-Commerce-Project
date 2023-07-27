using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Orders;

namespace Talabat.Repository.Data
{
	public static class StoreContextSeed
	{
		public static async Task SeedAsync(StoreContext context)
		{
			if (!context.ProductBrands.Any())
			{
				var brandData = File.ReadAllText("../Talabat.Repository/Data/SeedingData/brands.json");
				var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandData);
				if (brands != null & brands.Count > 0)
				{
					foreach (var b in brands)
					{
						await context.Set<ProductBrand>().AddAsync(b);
					}
					await context.SaveChangesAsync();

				}

			}
			if (!context.ProductTypes.Any())
			{
				var typeData = File.ReadAllText("../Talabat.Repository/Data/SeedingData/types.json");
				var types = JsonSerializer.Deserialize<List<ProductType>>(typeData);
				if (types != null & types.Count > 0)
				{

					foreach (var t in types)
					{
						await context.Set<ProductType>().AddAsync(t);
					}
					await context.SaveChangesAsync();
				}
			}
			if (!context.Products.Any())
			{
				var productData = File.ReadAllText("../Talabat.Repository/Data/SeedingData/products.json");
				var products = JsonSerializer.Deserialize<List<Product>>(productData);
				if (products != null & products.Count > 0)
				{ 
					foreach (var p in products)
					{
						await context.Set<Product>().AddAsync(p);
					}
				    await context.SaveChangesAsync();
			    }
			}
			if (!context.DeliveryMethods.Any())
			{
				var DeliveryMethodData = File.ReadAllText("../Talabat.Repository/Data/SeedingData/delivery.json");
				var deliveryMethods = JsonSerializer.Deserialize<List<DeliveryMethod>>(DeliveryMethodData);
				if (deliveryMethods != null & deliveryMethods.Count > 0)
				{
					foreach (var d in deliveryMethods)
					{
						await context.Set<DeliveryMethod>().AddAsync(d);
					}
					await context.SaveChangesAsync();
				}
			}

		}


	}
}
