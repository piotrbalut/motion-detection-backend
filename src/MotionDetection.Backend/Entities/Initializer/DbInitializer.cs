using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using MotionDetection.Backend.Entities.Initializer.Models;
using MotionDetection.Backend.Models.Database;
using Newtonsoft.Json;

namespace MotionDetection.Backend.Entities.Initializer
{
	//Move in the future to management portal
	public class DbInitializer
	{
		private const string DbInitializerConfigurationKey = "DbInitializer";

		public static async void InitializeAsync(
			CameraDbContext context,
			IConfiguration configuration,
			UserManager<User> userManager)
		{
			var data = JsonConvert.DeserializeObject<InitializeModel>(
				configuration.GetSection(DbInitializerConfigurationKey).Value);

			if (context.Cameras.Any() ||
			    context.Locations.Any() ||
			    context.Users.Any())
			{
				return;
			}

			//Users
			foreach (var user in data.Users)
			{
				await userManager.CreateAsync(user);
			}
			context.SaveChanges();

			//Locations
			foreach (var location in data.Locations)
			{
				context.Locations.Add(location);
				context.SaveChanges();
			}

			//User <-> Cameras
			foreach (var userCamera in data.UserCameras)
			{
				var userId = context.Users.Single(i => i.UserName == userCamera.Item1).Id;
				var cameraId = context.Cameras.Single(i => i.Key == userCamera.Item2).CameraId;

				context.UserCameras.Add(
					new UserCamera
					{
						UserId = userId,
						CameraId = cameraId
					});
				context.SaveChanges();
			}
        }
	}
}