using System;

namespace MotionDetection.Backend.Helpers
{
	public static class AccountCodeHelper
	{
		public static string GenerateAccountConfirmationCode()
		{
			const int min = 1000;
			const int max = 9999;
			var rdm = new Random();
			return rdm.Next(min, max).ToString();
		}
	}
}