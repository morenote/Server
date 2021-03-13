using Microsoft.VisualStudio.TestTools.UnitTesting;

using MoreNote.Common.Cryptography;

using System;
using System.Collections.Generic;
using System.Text;

namespace MoreNote.Common.Cryptography.Tests
{
    [TestClass()]
    public class Base64HelperTests
    {
        [TestMethod()]
        public void EncodeTest()
        {
            string str = "127.0.0.1";
            var base64 = Base64Helper.Encode(str);
            Console.WriteLine(base64);
        }

        [TestMethod()]
        public void FromBase64StringTest()
        {

        }
    }
}