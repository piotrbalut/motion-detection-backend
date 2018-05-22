using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MotionDetection.Backend.Entities;
using MotionDetection.Backend.Helpers;
using MotionDetection.Backend.Interfaces.Resources;
using MotionDetection.Backend.Interfaces.Services;
using MotionDetection.Backend.Models.Database;
using MotionDetection.Backend.Models.Dto;

namespace MotionDetection.Backend.Controllers
{
	[Route("[controller]/[action]")]
	public class AccountController : BaseController
	{
		private const string AccountCodeExpireMinutesKey = "AccountCodeExpireMinutes";
		private readonly IJwtService _jwtService;
		private readonly IMailService _mailService;
		private readonly SignInManager<User> _signInManager;

		public AccountController(
			UserManager<User> userManager,
			SignInManager<User> signInManager,
			IJwtService jwtService,
			IMailService mailService,
			ILoggerFactory loggerFactory,
			IMapper mapper,
            IConfiguration configuration,
			ICustomDateTime customDateTime,
			ICommonResource commonResource
		)
			: base(userManager, 
			       loggerFactory.CreateLogger(nameof(AccountController)),
				   mapper,
			       configuration, 
			       customDateTime, 
			       commonResource)
		{
			_signInManager = signInManager;
			_jwtService = jwtService;
			_mailService = mailService;
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
		public async Task<object> Confirm(
			[FromBody] AccountConfirmationDto model)
		{
			try
			{
				int accountCodeId;
				if (model.Password.IsNullOrWhiteSpace() ||
				    model.Email.IsNullOrWhiteSpace() ||
				    model.Code.IsNullOrWhiteSpace())
				{
					return BadRequest();
				}

                using (var db = new CameraDbContext())
                {
                    var accountCode = await db.AccountCodes.LastOrDefaultAsync(
		                                  ac => ac.Code == model.Code && 
		                                        ac.Email == model.Email &&
		                                        ac.ValidTo >= CustomDateTime.DateTimeNow &&
		                                        ac.DateOfUse == null);
	                if (accountCode == null)
	                {
		                return BadRequest();
                    }

	                accountCodeId = accountCode.AccountCodeId;
				}
				
				var user = await UserManager.FindByEmailAsync(model.Email);
				if (user == null)
				{
					return Unauthorized();
				}

				var addPasswordResult = await UserManager.AddPasswordAsync(user, model.Password);
				if (addPasswordResult.Succeeded)
				{
					await _signInManager.SignInAsync(user, false);
					using (var db = new CameraDbContext())
					{
						var accountCode = await db.AccountCodes.FindAsync(accountCodeId);
						accountCode.DateOfUse = CustomDateTime.DateTimeNow;
						await db.SaveChangesAsync();
					}
                    return await _jwtService.GenerateTokenAsync(model.Email, user);
				}

				Logger.LogWarning(addPasswordResult.Errors.ToString());
				return StatusCode(500);
			}
			catch (Exception e)
			{
				Logger.LogError(e, UnknownError.Name);
				return StatusCode(500);
			}
		}

		[HttpPost]
		public async Task<object> ConfirmationToken(
			[FromBody] AccountConfirmationCodeDto model)
		{
			try
			{
				if (!RegexUtilities.IsValidEmail(model.Email))
				{
					return BadRequest(CommonResource.RequestInvalidEmail);
				}

				var user = await UserManager.FindByEmailAsync(model.Email);
				if (user == null)
				{
					return Unauthorized();
				}

				var code = AccountCodeHelper.GenerateAccountConfirmationCode();
				var accountCodeExpireMinutes = Convert.ToDouble(Configuration.GetSection(AccountCodeExpireMinutesKey).Value);

				if (_mailService.SendConfirmationCode(model.Email, code))
				{
					using (var db = new CameraDbContext())
					{
						db.AccountCodes.Add(
							new AccountCode
							{
								Email = model.Email,
								Code = code,
								ValidTo = CustomDateTime.DateTimeNow.AddMinutes(accountCodeExpireMinutes)
							});
						db.SaveChanges();
					}

					return Ok();
                }

				return StatusCode(500);
			}
			catch (Exception e)
			{
				Logger.LogError(e, UnknownError.Name);
				return StatusCode(500);
			}
		}
	}
}