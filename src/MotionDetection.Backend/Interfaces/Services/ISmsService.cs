namespace MotionDetection.Backend.Interfaces.Services
{
	public interface ISmsService
	{
		bool SendAuthCode(
			string phoneNumber,
			string code);
	}
}