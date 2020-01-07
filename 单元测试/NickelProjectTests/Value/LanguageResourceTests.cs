using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoreNote.Value;
using System;
using System.Collections.Generic;
using System.Text;

namespace MoreNote.Value.Tests
{
    [TestClass()]
    public class LanguageResourceTests
    {
        [TestMethod()]
        public void GetMsgTest()
        {
            Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"));
            var a=  LanguageResource.GetMsg();
            Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"));
            var b = LanguageResource.GetMsg();
            if (b.ContainsKey("donate"))
            {

            }
            else
            {
                Assert.Fail();
            }
            Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"));
          //  Assert.Fail();
        }
    }
}