using Talabat.Core.Entities.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Talabat.API.Controllers;
using Talabat.API.DTO;
using Talabat.API.Errors;
using Talabat.Core.Services;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Talabat.API.Extensions;
using AutoMapper;
using Talabat.API.Helpers;

namespace Talabat.APIs.Controllers
{

	public class AccountsController : BaseAPIController
	{
		private readonly UserManager<AppUser> _userManager;
		private readonly SignInManager<AppUser> _signInManager;
		private readonly ITokenService _tokenService;
		private readonly IMapper _mapper;

		public AccountsController(UserManager<AppUser> userManager,SignInManager<AppUser> signInManager, ITokenService tokenService, IMapper mapper)
		{
			_userManager = userManager;
			_signInManager = signInManager;
			_tokenService = tokenService;
			_mapper = mapper;
		}
		[HttpPost("login")]
		public async Task<IActionResult> Login(LoginDTO loginD)
		{
			var user = await _userManager.FindByEmailAsync(loginD.Email);
			if (user == null) return Unauthorized(new ApiResponse(401));
			var result = await _signInManager.CheckPasswordSignInAsync(user, loginD.Password, false);
			if (!result.Succeeded) return Unauthorized(new ApiResponse(401));
			return Ok(new UserDTO()
			{
				DisplayName = user.DisplayName,
				Email = user.Email,
				Token = await _tokenService.CreateTokenAsync(user,_userManager)
			}) ;
		}

		[HttpPost("register")]
		public async Task<IActionResult> Register(RegisterDTO registerD)
		{
			if (CheckEmailExists(registerD.Email).Result.Value) return BadRequest(new ApiVaildationErrorResponse() {Errors = new string[] {"This Email is already exist !"} });
			var user = new AppUser()
			{
				DisplayName = registerD.DisplayName,
				Email = registerD.Email,
				PhoneNumber = registerD.PhoneNumber,
				UserName = registerD.Email.Split("@")[0]
			};
			var result = await _userManager.CreateAsync(user, registerD.Password);
			if (!result.Succeeded) return BadRequest(new ApiResponse(400));
			return Ok(new UserDTO()
			{
				DisplayName = user.DisplayName,
				Email = user.Email,
				Token = await _tokenService.CreateTokenAsync(user, _userManager)
			});
		}

		[Authorize]
		[CashedAttribute(600)]
		[HttpGet]
		public async Task<IActionResult> GetCurrentUser()
		{
			var email = User.FindFirstValue(ClaimTypes.Email);
			var user = await _userManager.FindByEmailAsync(email);
			return Ok(new UserDTO()
			{
				DisplayName = user.DisplayName,
				Email = user.Email,
				Token = await _tokenService.CreateTokenAsync(user, _userManager)
			});
		}
		[Authorize]
		[CashedAttribute(600)]
		[HttpGet("address")]
		public async Task<IActionResult> GetUserAddress()
		{
			//get the address and mapped it from address to addressDTO to display it to frontend user automaticaly
			var user = await _userManager.FindUserWithAddressByEmailAsync(User);
			var address = _mapper.Map<Address, AddressDTO>(user.Address);
			return Ok(address);
		}
		[Authorize]
		[HttpPut("address")]
		public async Task<IActionResult> UpdateUserAddress(AddressDTO updatedAddress)
		{
			var address = _mapper.Map<AddressDTO,Address>(updatedAddress);
			var user = await _userManager.FindUserWithAddressByEmailAsync(User);
			address.Id = user.Address.Id;
			user.Address = address;
			var result = await _userManager.UpdateAsync(user);
			if(!result.Succeeded) return BadRequest(new ApiResponse(400));
			return Ok(updatedAddress);
 
		}
		[CashedAttribute(600)]
		[HttpGet("emailexists")]
		public async Task<ActionResult<bool>> CheckEmailExists(string email)
		{
			return await _userManager.FindByEmailAsync(email) is not null;
		}

	}
}
