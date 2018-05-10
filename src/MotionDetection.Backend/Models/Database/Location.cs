using System.Collections.Generic;

namespace MotionDetection.Backend.Models.Database
{
	public class Location
	{
		public int LocationId { get; set; }
		public string Name { get; set; }
		public List<Camera> Cameras { get; set; }
	}
}