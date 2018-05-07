using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace MotionDetection.Backend.Controllers
{
	//[Produces("application/json")]
	[Route("api/Account")]
	public class AccountController : Controller
	{
		private readonly IConfiguration _configuration;
		private readonly SignInManager<IdentityUser> _signInManager;
		private readonly UserManager<IdentityUser> _userManager;

		public AccountController(
			UserManager<IdentityUser> userManager,
			SignInManager<IdentityUser> signInManager,
			IConfiguration configuration
		)
		{
			_userManager = userManager;
			_signInManager = signInManager;
			_configuration = configuration;
		}
	}
}