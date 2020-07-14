using Microsoft.VisualStudio.TestTools.UnitTesting;

using MoreNote.Common.Utils;

using System;
using System.Collections.Generic;
using System.Text;

namespace MoreNote.Common.Utils.Tests
{
    [TestClass()]
    public class MyConvertTests
    {
        [TestMethod()]
        public void HexToLongTest()
        {
            long number = 1269544638696525824;
            string hex = number.ToString("x");
            Console.WriteLine(hex);

        }
    }
}