using System;
using System.Net;
using Microsoft.Extensions.Options;
using MotionDetection.Backend.Interfaces.Resources;
using MotionDetection.Backend.Interfaces.Services;
using MotionDetection.Backend.Models.Mailgun;
using RestSharp;
using RestSharp.Authenticators;

namespace MotionDetection.Backend.Services
{
	public class MailService : IMailService
	{
		private const string ApiParameterKey = "api";
		private const string DomainParameterKey = "domain";
		private const string DomainResourceKey = "{domain}/";
		private const string MessagesParameterKey = "messages";
		private const string FromParameterKey = "from";
		private const string ToParameterKey = "to";
		private const string SubjectParameterKey = "subject";
		private const string TextParameterKey = "text";
		private readonly RestClient _client;
		private readonly ICommonResource _commonResource;
		private readonly IOptions<MailgunSettings> _mailgunSettings;

		public MailService(
			IOptions<MailgunSettings> mailgunSettings,
			ICommonResource commonResource)
		{
			_mailgunSettings = mailgunSettings;
			_commonResource = commonResource;
			_client = new RestClient
			{
				BaseUrl = new Uri(_mailgunSettings.Value.MailgunApiUrl),
				Authenticator = new HttpBasicAuthenticator(ApiParameterKey, _mailgunSettings.Value.MailgunApiKey)
			};
		}

		#region IMailService Members

		public bool SendConfirmationCode(
			string to,
			string code)
		{
			var request = new RestRequest();
			request.AddParameter(DomainParameterKey, _mailgunSettings.Value.MailgunDomain, ParameterType.UrlSegment);
			request.Resource = DomainResourceKey + MessagesParameterKey;
			request.AddParameter(FromParameterKey, _mailgunSettings.Value.MailgunFrom);
			request.AddParameter(ToParameterKey, to);
			request.AddParameter(SubjectParameterKey, $"{_commonResource.AppName} - {_commonResource.MailServiceConfirmationCodeTitle}");
			request.AddParameter(TextParameterKey, $"{_commonResource.MailServiceConfirmationCodeMessage}{code}");
			request.Method = Method.POST;
			var result = _client.Execute(request);
			return result.StatusCode == HttpStatusCode.OK;
		}

		#endregion
	}
}