using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Identity;

namespace Talabat.Repository.Identity
{
	public class AppIdentityDbContextDataSeed
	{
		public static async Task SeedUsersAsync(UserManager<AppUser>userManager)
		{
			if (!userManager.Users.Any())
			{
				var user = new AppUser()
				{
					DisplayName = "Lobna Raafat",
					Email = "lobnaraafat21@gmail.com",
					UserName = "lobnaraafat21",
					PhoneNumber = "01009683650"
				};
				await userManager.CreateAsync(user,"Passwordlr");
				await userManager.UpdateAsync(user);

			}
		}
	}
}
