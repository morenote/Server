using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoreNote.Common.Util;
using System;
using System.Collections.Generic;
using System.Text;

namespace MoreNote.Common.Util.Tests
{
    [TestClass()]
    public class RndNumTests
    {
        [TestMethod()]
        public void CreatRndNumTest()
        {
            String str = RndNum.CreatRndNum(32);
            Console.WriteLine(str);

        }

        [TestMethod()]
        public void CreatSafeNumTest()
        {
            string safe = RndNum.CreatSafeNum();
            Console.WriteLine(safe);

        }
    }
}