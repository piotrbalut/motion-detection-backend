using System.Collections.Generic;

namespace MotionDetection.Backend.Models.Database
{
	public class Camera
	{
		public int CameraId { get; set; }
		public string Key { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }
		public int LocationId { get; set; }
		public Location Location { get; set; }
		public virtual ICollection<UserCamera> UserCameras { get; set; }
	}
}