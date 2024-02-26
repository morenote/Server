using Microsoft.VisualStudio.TestTools.UnitTesting;

using MoreNote.Common.ExtensionMethods;

using System;

namespace MoreNote.Common.Utils.Tests
{
	[TestClass()]
	public class MyConvertTests
	{
		[TestMethod()]
		public void HexToLongTest()
		{
			long? number = 1269544638696525824;
			string hex = number.ToHex();
			Console.WriteLine(hex);

		}
	}
}