using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoreNote.Common.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace MoreNote.Common.Utils.Tests
{
    [TestClass()]
    public class RandomToolTests
    {
        [TestMethod()]
        public void CreatSafeSaltTest()
        {
            string salt = RandomTool.CreatSafeSaltBase64(32);
            Console.WriteLine(salt);
        }

        [TestMethod()]
        public void CreatSafeSaltTest1()
        {

        }
    }
}