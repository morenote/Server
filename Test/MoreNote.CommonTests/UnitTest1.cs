using Microsoft.VisualStudio.TestTools.UnitTesting;

using MoreNote.Common.ExtensionMethods;

namespace 通用类库测试
{
	[TestClass]
	public class UnitTest1
	{
		[TestMethod]
		public void TestMethod1()
		{
			long? number = long.MaxValue;
			string hex = number.ToHex();
			System.Console.WriteLine(hex);
		}
	}
}
