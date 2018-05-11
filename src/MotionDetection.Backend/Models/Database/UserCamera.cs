namespace MotionDetection.Backend.Models.Database
{
	public class UserCamera
	{
		public string UserId { get; set; }
		public User User { get; set; }
		public int CameraId { get; set; }
		public Camera Camera { get; set; }
	}
}