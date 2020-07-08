using Microsoft.VisualStudio.TestTools.UnitTesting;

using MoreNote.Common.GoogleAuthenticator;

using System;
using System.Collections.Generic;
using System.Text;

namespace MoreNote.Common.GoogleAuthenticator.Tests
{
    [TestClass()]
    public class GoogleAuthenticatorHelperTests
    {
        [TestMethod()]
        public void GetqrCodeImageUrlTest()
        {
            string url= GoogleAuthenticatorHelper.GetqrCodeImageUrl();
            Console.WriteLine(url);
        }
    }
}