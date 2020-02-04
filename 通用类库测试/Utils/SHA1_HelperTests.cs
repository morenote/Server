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
            Console.WriteLine(DateTime.Now);
            int i=0;
            string x = SHAEncrypt_Helper.Hash256Encrypt("wKvbMYSvdnqXZAUq" + "QuMF3qJs4WrLJPB24bEzdLUlcxik6dDa");
            for (i = 0; i < 100000; i++)
            {
                x=SHAEncrypt_Helper.Hash256Encrypt(x);
            }
            Console.WriteLine("计算数量="+i);
            Console.WriteLine(DateTime.Now);
            string c = SHAEncrypt_Helper.Hash256Encrypt("wKvbMYSvdnqXZAUq" + "QuMF3qJs4WrLJPB24bEzdLUlcxik6dDa");
           
            Console.WriteLine(c);
            c= SHAEncrypt_Helper.Hash1Encrypt("sssss");
            Console.WriteLine(c);
            c = SHAEncrypt_Helper.MD5Encrypt("sssss");
            Console.WriteLine(c);
        }
    }
}