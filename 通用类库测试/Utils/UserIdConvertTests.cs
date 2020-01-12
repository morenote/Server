using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoreNote.Common.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace MoreNote.Common.Utils.Tests
{
    [TestClass()]
    public class UserIdConvertTests
    {
        [TestMethod()]
        public void ConvertStrToLongTest()
        {

            long a = 1213656203658399745;
            string hex = a.ToString("x");
            Console.WriteLine(hex);
            long b = MyConvert.ConvertStrToLong(hex);
            Console.WriteLine(b);//1213656226102120449
        }
    }
}