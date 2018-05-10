using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MotionDetection.Backend.Entities;
using MotionDetection.Backend.Interfaces.Services;
using MotionDetection.Backend.Models.Database;
using MotionDetection.Backend.Models.Dto;
using MotionDetection.Backend.Models.Plivo;

namespace MotionDetection.Backend.Controllers
{
	[Route("[controller]/[action]")]
	public class AccountController : BaseController
	{
		private readonly SignInManager<IdentityUser> _signInManager;
		private readonly IJwtService _jwtService;
		private readonly ISmsService _smsService;

		public AccountController(
			UserManager<IdentityUser> userManager,
			SignInManager<IdentityUser> signInManager,
			IOptions<PlivoAuth> jwtAuthentication,
			IJwtService jwtService,
			ISmsService smsService
		) : base(userManager)
		{
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
				var appUser = UserManager.Users.SingleOrDefault(r => r.Email == model.Email);
				return await _jwtService.GenerateTokenAsync(model.Email, appUser);
			}

			return StatusCode(500);
		}

		[HttpPost]
		public async Task<object> Register(
			[FromBody] RegisterDto model)
		{
			using (var db = new CameraDbContext())
			{
				db.Locations.Add(
					new Location()
					{
						Name = "Home",
						Cameras = new List<Camera>()
						{
							new Camera() {Title = "1"},
							new Camera() {Title = "2"}
						}
					});
				var count = db.SaveChanges();

				foreach (var camera in db.Cameras)
				{
				}
			}

			//var user = new IdentityUser
			//{
			//	UserName = model.Email,
			//	Email = model.Email
			//};

			//var result = await _userManager.CreateAsync(user);

			//if (result.Succeeded)
			//{
			//	await _userManager.SetPhoneNumberAsync(user, model.PhoneNumber);
			//	await _signInManager.SignInAsync(user, false);
			//	return Ok();
			//}

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

			var result = await UserManager.CreateAsync(user, model.PhoneNumber);

			if (result.Succeeded)
			{
				await _signInManager.SignInAsync(user, false);
				return await _jwtService.GenerateTokenAsync(model.Email, user);
			}

			return StatusCode(500);
		}
	}
}