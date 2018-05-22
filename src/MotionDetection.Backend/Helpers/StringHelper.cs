namespace MotionDetection.Backend.Helpers
{
	public static class StringHelper
	{
		public static bool IsNullOrEmpty(
			this string @string)
			=> string.IsNullOrEmpty(@string);

		public static bool IsNullOrWhiteSpace(
			this string @string)
			=> string.IsNullOrWhiteSpace(@string);
	}
}