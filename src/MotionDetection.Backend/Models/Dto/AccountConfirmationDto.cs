namespace MotionDetection.Backend.Models.Dto
{
	public class AccountConfirmationDto
	{
		/// <summary>
        /// Email to verify account
        /// </summary>
		public string Email { get; set; }
		/// <summary>
        /// Password for account
        /// </summary>
		public string Password { get; set; }
		/// <summary>
        /// Code to activate account
        /// </summary>
		public string Code { get; set; }
	}
}