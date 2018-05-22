using System.Collections.Generic;

namespace MotionDetection.Backend.Models.Dto
{
	public class RegisterDto
	{
		public string Email { get; set; }
		public string PhoneNumber { get; set; }
		public IEnumerable<string> CameraKeys { get; set; }
	}
}