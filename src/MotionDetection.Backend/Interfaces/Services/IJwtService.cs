using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace MotionDetection.Backend.Interfaces.Services
{
	public interface IJwtService
	{
		Task<object> GenerateTokenAsync(
			string email,
			IdentityUser user);
	}
}