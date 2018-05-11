using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MotionDetection.Backend.Entities;
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
			CameraDbContext cameraDbContext)
			: base(userManager)
		{
			_cameraDbContext = cameraDbContext;
		}

		// GET api/cameras
		[HttpGet]
		public async Task<IEnumerable<CameraDto>> Get()
		{
			//Mapper.Initialize(cfg => cfg.CreateMap<Camera, CameraDto>());
			//var cameras = _cameraDbContext.Cameras.ToArray();
			return Mapper.Map<CameraDto[]>(null);
		}

		// GET api/cameras/5
		[HttpGet("{id}")]
		public string Get(
			int id)
			=> "value";
	}
}