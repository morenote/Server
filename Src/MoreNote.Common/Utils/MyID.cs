using System;

namespace MoreNote.Common.Utils
{
	public class MyID
	{
		public static string CreatGuid()
		{
			string guid = Guid.NewGuid().ToString();
			return guid;
		}

		public static string CreatUUID()
		{
			return System.Guid.NewGuid().ToString("N");
		}
	}
}
