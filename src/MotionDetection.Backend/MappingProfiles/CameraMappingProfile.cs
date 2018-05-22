using AutoMapper;
using MotionDetection.Backend.Models.Database;
using MotionDetection.Backend.Models.Dto;

namespace MotionDetection.Backend.MappingProfiles
{
	public class CameraMappingProfile : Profile
	{
		public CameraMappingProfile() { CreateMap<Camera, CameraDto>(); }
	}
}