using System;

namespace MotionDetection.Backend.Models.Database
{
	public class AccountCode
	{
		public int AccountCodeId { get; set; }
		public string Code { get; set; }
		public string Email { get; set; }
		public DateTime ValidTo { get; set; }
		public DateTime? DateOfUse { get; set; }
	}
}