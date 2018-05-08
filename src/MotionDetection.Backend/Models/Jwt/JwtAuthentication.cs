namespace MotionDetection.Backend.Models.Jwt
{
	public class JwtAuthentication
	{
		public string SecurityKey { get; set; }
		public string ValidIssuer { get; set; }
		public int JwtExpireDays { get; set; }
	}
}