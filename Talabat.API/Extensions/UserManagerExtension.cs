using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Talabat.Core.Entities.Identity;

namespace Talabat.API.Extensions
{
	public static class UserManagerExtension
	{
		public static async Task<AppUser?> FindUserWithAddressByEmailAsync(this UserManager<AppUser> userManager, ClaimsPrincipal User)
		{
			//Get the address of the login user
			var email = User.FindFirstValue(ClaimTypes.Email);
			var user = await userManager.Users.Include(u => u.Address).FirstOrDefaultAsync(u => u.Email == email);
			return user;
		}
	}
}
