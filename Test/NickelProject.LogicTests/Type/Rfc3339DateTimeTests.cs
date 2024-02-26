using Microsoft.VisualStudio.TestTools.UnitTesting;

using MoreNote.Common.Utils;

using System;

namespace leanote.Common.Type.Tests
{
	[TestClass()]
	public class Rfc3339DateTimeTests
	{
		[TestMethod()]
		public void Rfc3339DateTimeTest()
		{
			Rfc3339DateTime rfc3339DateTime = new Rfc3339DateTime();
			Console.WriteLine(rfc3339DateTime.ToRfc3339String());

		}

		[TestMethod()]
		public void Rfc3339DateTimeTest1()
		{

		}

		[TestMethod()]
		public void ToRfc3339StringTest()
		{

		}

		[TestMethod()]
		public void ToDataTimeStringTest()
		{

		}

		[TestMethod()]
		public void EqualsTest()
		{

		}

		[TestMethod()]
		public void GetHashCodeTest()
		{

		}

		[TestMethod()]
		public void ToStringTest()
		{

		}
	}
}