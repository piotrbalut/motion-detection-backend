namespace MotionDetection.Backend.Models.Database
{
	public class Camera
	{
		public int CameraId { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }
		public int LocationId { get; set; }
		public Location Location { get; set; }
	}
}