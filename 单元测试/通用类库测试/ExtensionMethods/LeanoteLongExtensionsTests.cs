using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoreNote.Common.ExtensionMethods;
using MoreNote.Common.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace MoreNote.Common.ExtensionMethods.Tests
{
    [TestClass()]
    public class LeanoteLongExtensionsTests
    {
        [TestMethod()]
        public void ToLongByHexTest()
        {
            long id=SnowFlakeNet.GenerateSnowFlakeID();
            Console.WriteLine(id);
            string hex24=id.ToHex24();
            Console.WriteLine(hex24);
            long id24=hex24.ToLongByHex();
            Console.WriteLine(id24);
        }
    }
}