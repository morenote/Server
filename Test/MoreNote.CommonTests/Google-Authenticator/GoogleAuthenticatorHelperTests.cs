using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;

namespace MoreNote.Common.GoogleAuthenticator.Tests
{
	[TestClass()]
	public class GoogleAuthenticatorHelperTests
	{
		[TestMethod()]
		public void GetqrCodeImageUrlTest()
		{
			string url = GoogleAuthenticatorHelper.GetqrCodeImageUrl();
			Console.WriteLine(url);
		}
	}
}