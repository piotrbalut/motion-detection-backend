using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MotionDetection.Backend.Interfaces.Resources;
using MotionDetection.Backend.Interfaces.Services;
using MotionDetection.Backend.Models.Database;

namespace MotionDetection.Backend.Controllers
{
	public abstract class BaseController : Controller
    {
		protected readonly EventId UnknownError = new EventId(1, "UnknownError");
        protected readonly UserManager<User> UserManager;
		protected readonly ILogger Logger;
	    protected readonly IMapper Mapper;
        protected readonly IConfiguration Configuration;
		protected readonly ICustomDateTime CustomDateTime;
	    protected readonly ICommonResource CommonResource;
		
        public BaseController(
			UserManager<User> userManager,
			ILogger logger,
			IMapper mapper,
            IConfiguration configuration,
			ICustomDateTime customDateTime,
	        ICommonResource commonResource)
		{
			UserManager = userManager;
			Logger = logger;
			Mapper = mapper;
			Configuration = configuration;
			CustomDateTime = customDateTime;
			CommonResource = commonResource;
		}

		protected async Task<string> GetUserIdAsync()
			=> (await UserManager.GetUserAsync(HttpContext.User)).Id;
	}
}