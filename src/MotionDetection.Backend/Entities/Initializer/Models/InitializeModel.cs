using System;
using System.Collections.Generic;
using MotionDetection.Backend.Models.Database;

namespace MotionDetection.Backend.Entities.Initializer.Models
{
	public class InitializeModel
	{
		public List<User> Users { get; set; }
		public List<Location> Locations { get; set; }
		/// <summary>
        /// UserName, CameraKey
        /// </summary>
		public IEnumerable<Tuple<string, string>> UserCameras { get; set; }
	}
}