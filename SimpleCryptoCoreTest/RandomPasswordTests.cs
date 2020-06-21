using Microsoft.VisualStudio.TestTools.UnitTesting;

using SimpleCrypto.Core;

using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleCrypto.Core.Tests
{
    [TestClass()]
    public class RandomPasswordTests
    {
        [TestMethod()]
        public void GenerateTest()
        {
           string low= RandomPassword.Generate(PasswordGroup.Lowercase);
           string hight= RandomPassword.Generate(32,new PasswordGroup[] { PasswordGroup.Lowercase,PasswordGroup.Uppercase,PasswordGroup.Numeric,PasswordGroup.Special});
            Console.WriteLine(low);
            Console.WriteLine(hight);
        }

    }
}