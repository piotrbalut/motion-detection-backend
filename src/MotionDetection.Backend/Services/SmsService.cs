using System.Collections.Generic;
using Microsoft.Extensions.Options;
using MotionDetection.Backend.Interfaces.Services;
using MotionDetection.Backend.Models.Plivo;
using Plivo;

namespace MotionDetection.Backend.Services
{
	public class SmsService : ISmsService
	{
		private readonly PlivoApi _plivoApi;
		private readonly IOptions<PlivoAuth> _plivoAuth;
		private readonly IOptions<PlivoSettings> _plivoSettings;

		public SmsService(
			IOptions<PlivoAuth> plivoAuth,
			IOptions<PlivoSettings> plivoSettings)
		{
			_plivoAuth = plivoAuth;
			_plivoSettings = plivoSettings;
			_plivoApi = new PlivoApi(_plivoAuth.Value.AuthId, _plivoAuth.Value.AuthToken);
		}

		#region ISmsService Members

		public bool SendAuthCode(
			string phoneNumber,
			string code)
		{
			var response = _plivoApi.Message.Create(
					_plivoSettings.Value.Src,
					new List<string> {phoneNumber},
					"Code: "
				);

			return true;
		}

		#endregion
	}
}