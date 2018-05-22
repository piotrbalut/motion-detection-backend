using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace MotionDetection.Backend.Models.Database
{
	public class User : IdentityUser
	{
		public virtual ICollection<UserCamera> UserCameras { get; set; }
	}
}