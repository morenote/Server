﻿using Microsoft.VisualStudio.TestTools.UnitTesting;

using MoreNote.Common.ExtensionMethods;

using System;

namespace MoreNote.Common.Utils.Tests
{
	[TestClass()]
	public class UserIdConvertTests
	{
		[TestMethod()]
		public void ConvertStrToLongTest()
		{

			long? a = 1213656203658399745;
			string hex = a.ToHex();
			Console.WriteLine(hex);
			long? b = hex.ToLongByHex();
			Console.WriteLine(b);//1213656226102120449
		}
	}
}