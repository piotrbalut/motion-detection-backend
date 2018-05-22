using System;
using MotionDetection.Backend.Interfaces.Services;

namespace MotionDetection.Backend.Services
{
	public class CustomDateTime : ICustomDateTime
	{
		#region ICustomDateTime Members

		public DateTime DateTimeNow => DateTime.Now;

		/// <summary>
		///     Dispose of class and parent classes
		/// </summary>
		public void Dispose()
			=> GC.SuppressFinalize(this);

		#endregion
	}
}