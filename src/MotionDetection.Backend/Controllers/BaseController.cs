using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MotionDetection.Backend.Models.Database;

namespace MotionDetection.Backend.Controllers
{
	public class BaseController : Controller
	{
		protected readonly UserManager<User> UserManager;

		public BaseController(
			UserManager<User> userManager)
		{
			UserManager = userManager;
		}

		protected async Task<string> GetUserIdAsync()
			=> (await UserManager.GetUserAsync(HttpContext.User)).Id;
	}
}