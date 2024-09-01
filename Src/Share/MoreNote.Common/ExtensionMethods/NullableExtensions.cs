namespace MoreNote.Common.ExtensionMethods
{
	public static class NullableExtensions
	{
		public static int CompareTo(this long? numner1, long? number2)
		{
			if (number2 == numner1)
			{
				return 0;
			}
			if (numner1 > number2)
			{
				return 1;

			}
			return -1;
		}
	}
}
