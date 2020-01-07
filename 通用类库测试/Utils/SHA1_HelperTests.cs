using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoreNote.Common.Util;
using System;
using System.Collections.Generic;
using System.Text;

namespace MoreNote.Common.Util.Tests
{
    [TestClass()]
    public class SHA1_HelperTests
    {
        [TestMethod()]
        public void HashEncryptTest()
        {
            string c = Encrypt_Helper.Hash256Encrypt("wKvbMYSvdnqXZAUq" + "QuMF3qJs4WrLJPB24bEzdLUlcxik6dDa");
            Console.WriteLine(c);
            c= Encrypt_Helper.Hash1Encrypt("sssss");
            Console.WriteLine(c);
            c = Encrypt_Helper.MD5Encrypt("sssss");
            Console.WriteLine(c);
        }
    }
}