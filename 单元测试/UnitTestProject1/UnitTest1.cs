using Microsoft.VisualStudio.TestTools.UnitTesting;
using NickelProject.Logic.DB;
using NickelProject.Logic.Entity;
using NickelProject.Models;
using System;
using System.Linq;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            using (var db = new DataContext())
            {
                var userEntities = db.User
                    .Where(b => b.Userid=="a")
                    .OrderBy(b => b.Age)
                    .ToList();
                foreach(UserEntity userEntity in userEntities)
                {
                    Console.WriteLine(userEntity.UserName.ToString());
                    Assert.AreEqual(userEntity.UserName.ToString(),"a");
                }
            }

        }
    }
}
