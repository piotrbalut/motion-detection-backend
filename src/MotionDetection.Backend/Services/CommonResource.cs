using Microsoft.Extensions.Localization;
using MotionDetection.Backend.Interfaces.Resources;
using MotionDetection.Backend.Resources;

namespace MotionDetection.Backend.Services
{
	public class CommonResource : ICommonResource
	{
		private readonly IStringLocalizer<SharedResource> _localizer;

		public CommonResource(
			IStringLocalizer<SharedResource> localizer)
		{
			_localizer = localizer;
		}

		#region General
		
		public string AppName => GetString(nameof(AppName));

        #endregion

        #region Requests

        public string RequestInvalidEmail => GetString(nameof(RequestInvalidEmail));

        #endregion

        #region MailService

		public string MailServiceConfirmationCodeTitle => GetString(nameof(MailServiceConfirmationCodeTitle));
		public string MailServiceConfirmationCodeMessage => GetString(nameof(MailServiceConfirmationCodeMessage));

        #endregion
		
        private string GetString(
			string name)
			=> _localizer[name];
	}
}