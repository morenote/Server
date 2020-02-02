using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoreNote.Common.Util;
using System;
using System.Collections.Generic;
using System.Text;

namespace MoreNote.Common.Util.Tests
{
    [TestClass()]
    public class RandomToolTests
    {
        [TestMethod()]
        public void CreatSafeSaltTest()
        {
           string salt= RandomTool.CreatSafeSalt(32);
           Console.WriteLine(salt);
        }
    }
}