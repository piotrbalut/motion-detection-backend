using System;

namespace MotionDetection.Backend.Interfaces.Services
{
	public interface ICustomDateTime : IDisposable
	{
		DateTime DateTimeNow { get; }
	}
}