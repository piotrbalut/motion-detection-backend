using System;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MotionDetection.Backend.Entities;
using MotionDetection.Backend.Entities.Initializer;
using MotionDetection.Backend.Models.Database;

namespace MotionDetection.Backend
{
	public class Program
	{
		public static void Main(
			string[] args)
		{
			var host = BuildWebHost(args);

			using (var scope = host.Services.CreateScope())
			{
				var services = scope.ServiceProvider;
				try
				{
					var context = services.GetRequiredService<CameraDbContext>();
					var configuration = services.GetRequiredService<IConfiguration>();
					var userManager = services.GetRequiredService<UserManager<User>>();
                    DbInitializer.InitializeAsync(context, configuration, userManager);
				}
				catch (Exception ex)
				{
					var logger = services.GetRequiredService<ILogger<Program>>();
					logger.LogError(ex, "An error occurred while seeding the database.");
				}
			}
			
            host.Run();
        } 

		public static IWebHost BuildWebHost(
			string[] args)
			=> WebHost.CreateDefaultBuilder(args)
			          .ConfigureAppConfiguration(
				          (
					          builderContext,
					          config) =>
				          {
					          var env = builderContext.HostingEnvironment;
					          config.AddJsonFile("appsettings.json")
					                .AddJsonFile($"appsettings.{env.EnvironmentName}.json");
				          })
			          .ConfigureLogging((hostingContext, logging) =>
			          {
				          logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
				          logging.AddConsole();
				          logging.AddDebug();
			          })
                      .UseStartup<Startup>()
			          .Build();
	}
}