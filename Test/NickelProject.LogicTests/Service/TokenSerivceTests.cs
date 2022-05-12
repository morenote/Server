using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoreNote.Common.Utils;
using System;

namespace MoreNote.Logic.Service.Tests
{
    [TestClass()]
    public class TokenSerivceTests
    {
        //TokenSerivce tokenSerivce=new TokenSerivce();

        public TokenSerivceTests( )
        {
            //this.tokenSerivce = tokenSerivce;
        }

        [TestMethod()]
        public void GenerateTokenTest()
        {
            long? id = SnowFlakeNetService.GenerateSnowFlakeID();
            Console.WriteLine(id);
            //string token = tokenSerivce.GenerateToken(id,16);
            //Console.WriteLine(token);
        }
    }
}