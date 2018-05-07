namespace MotionDetection.Backend.Models
{
	public class JwtAuthentication
	{
		public string SecurityKey { get; set; }
		public string ValidIssuer { get; set; }
		public string ValidAudience { get; set; }
	}
}