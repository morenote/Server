using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;

namespace MoreNote.Common.Utils.Tests
{
	[TestClass()]
	public class SHA1_HelperTests
	{
		[TestMethod()]
		public void HashEncryptTest()
		{
			Console.WriteLine(DateTime.Now);
			int i = 0;
			string x = SHAEncryptHelper.Hash256Encrypt("wKvbMYSvdnqXZAUq" + "QuMF3qJs4WrLJPB24bEzdLUlcxik6dDa");
			for (i = 0; i < 100000; i++)
			{
				x = SHAEncryptHelper.Hash256Encrypt(x);
			}
			Console.WriteLine("计算数量=" + i);
			Console.WriteLine(DateTime.Now);
			string c = SHAEncryptHelper.Hash256Encrypt("wKvbMYSvdnqXZAUq" + "QuMF3qJs4WrLJPB24bEzdLUlcxik6dDa");

			Console.WriteLine(c);
			c = SHAEncryptHelper.Hash1Encrypt("sssss");
			Console.WriteLine(c);
			c = SHAEncryptHelper.MD5Encrypt("sssss");
			Console.WriteLine(c);
		}
	}
}