using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MotionDetection.Backend.Interfaces.Services;
using MotionDetection.Backend.Models.Dto;
using MotionDetection.Backend.Models.Plivo;

namespace MotionDetection.Backend.Controllers
{
	[Route("[controller]/[action]")]
	public class AccountController : Controller
	{
		private readonly IJwtService _jwtService;
		private readonly SignInManager<IdentityUser> _signInManager;
		private readonly ISmsService _smsService;
		private readonly UserManager<IdentityUser> _userManager;

		public AccountController(
			UserManager<IdentityUser> userManager,
			SignInManager<IdentityUser> signInManager,
			IOptions<PlivoAuth> jwtAuthentication,
			IJwtService jwtService,
			ISmsService smsService
		)
		{
			_userManager = userManager;
			_signInManager = signInManager;
			_jwtService = jwtService;
			_smsService = smsService;
		}

		[HttpPost]
		public async Task<object> Login(
			[FromBody] LoginDto model)
		{
			var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);

			if (result.Succeeded)
			{
				var appUser = _userManager.Users.SingleOrDefault(r => r.Email == model.Email);
				return await _jwtService.GenerateTokenAsync(model.Email, appUser);
			}

			return StatusCode(500);
		}

		[HttpPost]
		public async Task<object> Register(
			[FromBody] RegisterDto model)
		{
			var user = new IdentityUser
			{
				UserName = model.Email,
				Email = model.Email
			};

			var result = await _userManager.CreateAsync(user);

			if (result.Succeeded)
			{
				await _userManager.SetPhoneNumberAsync(user, model.PhoneNumber);
				await _signInManager.SignInAsync(user, false);
				return Ok();
			}

			return StatusCode(500);
		}

		[HttpPost]
		public async Task<object> Confirm(
			[FromBody] ConfirmAccountDto model)
		{
			var user = new IdentityUser
			{
				UserName = model.Email,
				Email = model.Email
			};

			var result = await _userManager.CreateAsync(user, model.PhoneNumber);

			if (result.Succeeded)
			{
				await _signInManager.SignInAsync(user, false);
				return await _jwtService.GenerateTokenAsync(model.Email, user);
			}

			return StatusCode(500);
		}
	}
}