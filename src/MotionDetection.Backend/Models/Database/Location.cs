using System.Collections.Generic;

namespace MotionDetection.Backend.Models.Database
{
	public class Location
	{
		public int LocationId { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }
		public double Latitude { get; set; }
		public double Longitude { get; set; }
        public List<Camera> Cameras { get; set; }
	}
}