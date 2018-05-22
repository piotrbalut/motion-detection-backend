using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MotionDetection.Backend.Entities;
using MotionDetection.Backend.Interfaces.Resources;
using MotionDetection.Backend.Interfaces.Services;
using MotionDetection.Backend.Models.Database;
using MotionDetection.Backend.Models.Dto;

namespace MotionDetection.Backend.Controllers
{
	[Authorize]
	[Route("api/[controller]")]
	public class CamerasController : BaseController
	{
		private readonly CameraDbContext _cameraDbContext;

		// GET api/cameras
		public CamerasController(
			UserManager<User> userManager,
			ILoggerFactory loggerFactory,
			IMapper mapper,
			CameraDbContext cameraDbContext,
			IConfiguration configuration,
			ICustomDateTime customDateTime,
			ICommonResource commonResource)
			: base(userManager, 
			       loggerFactory.CreateLogger(nameof(CamerasController)),
				   mapper,
			       configuration, 
			       customDateTime, 
			       commonResource)
		{
			_cameraDbContext = cameraDbContext;
		}

		// GET api/cameras
		[Authorize]
		[HttpGet]
		public async Task<IEnumerable<CameraDto>> Get()
		{
			var userId = await GetUserIdAsync();
			IQueryable<Camera> cameras;
			using (var db = new CameraDbContext())
			{
				cameras = db.Users.Where(p => p.Id == userId)
				          .SelectMany(p => p.UserCameras)
				          .Select(pc => pc.Camera);

				return Mapper.Map<CameraDto[]>(cameras.ToArray());
            }
		}

		// GET api/cameras/5
		[HttpGet("{id}")]
		public string Get(
			int id)
		{
			return null;
        }
	}
}