namespace MotionDetection.Backend.Interfaces.Services
{
	public interface IMailService
	{
		bool SendConfirmationCode(
			string mail,
			string code);
	}
}