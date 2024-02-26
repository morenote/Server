using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;

namespace MoreNote.Common.Utils.Tests
{
	[TestClass()]
	public class RndNumTests
	{
		[TestMethod()]
		public void CreatRndNumTest()
		{
			String str = RandomTool.CreatRandomString(32);
			Console.WriteLine(str);

		}

		[TestMethod()]
		public void CreatSafeNumTest()
		{
			string safe = RandomTool.CreatSafeRandomBase64();
			Console.WriteLine(safe);

		}
	}
}