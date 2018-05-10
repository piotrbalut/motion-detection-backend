using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace MotionDetection.Backend.Controllers
{
	public class BaseController : Controller
	{
		protected readonly UserManager<IdentityUser> UserManager;

		public BaseController(
			UserManager<IdentityUser> userManager)
		{
			UserManager = userManager;
		}

		protected async Task<string> GetUserIdAsync()
			=> (await UserManager.GetUserAsync(HttpContext.User)).Id;
	}
}