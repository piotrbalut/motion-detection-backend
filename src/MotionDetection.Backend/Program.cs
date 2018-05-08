using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace MotionDetection.Backend
{
	public class Program
	{
		public static void Main(
			string[] args)
			=> BuildWebHost(args).Run();

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
			          .UseStartup<Startup>()
			          .Build();
	}
}