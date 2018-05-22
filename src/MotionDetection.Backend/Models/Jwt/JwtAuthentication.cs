namespace MotionDetection.Backend.Models.Jwt
{
	public class JwtAuthentication
	{
		public string JwtKey { get; set; }
		public string JwtIssuer { get; set; }
		public int JwtExpireDays { get; set; }
	}
}