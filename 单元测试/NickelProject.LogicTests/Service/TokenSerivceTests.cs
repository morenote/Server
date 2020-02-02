using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoreNote.Common.Utils;
using System;

namespace MoreNote.Logic.Service.Tests
{
    [TestClass()]
    public class TokenSerivceTests
    {
        [TestMethod()]
        public void GenerateTokenTest()
        {
            long id = SnowFlake_Net.GenerateSnowFlakeID();
            Console.WriteLine(id);
            string token = TokenSerivce.GenerateToken(id,16);
            Console.WriteLine(token);
        }
    }
}