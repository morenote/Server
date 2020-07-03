using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoreNote.Common.Utils;
using MoreNote.Controllers;
using MoreNote.Logic.DB;
using MoreNote.Logic.Entity;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MoreNote.Controllers.Tests
{
    [TestClass()]
    public class AuthControllerTests
    {
        [TestMethod()]
        public void DoLoginTest()
        {
            //using (var db= DataContext.getDataContext())
            //{
            //    User user = db.User.Where(b => b.Username.Equals("hyfree")).FirstOrDefault();
            //    Console.WriteLine(user.Email);
            //    var authorizations= user.Jurisdiction;
            //    if (authorizations==null)
            //    {
            //        //Assert.Fail("authorizations==null");
            //        authorizations = new List<Authorization>();
            //        authorizations.Add(new Authorization(SnowFlake_Net.GenerateSnowFlakeID(), "user", "12"));


            //    }
            //    if (authorizations.Any())
            //    {
            //        foreach (var item in authorizations)
            //        {
            //            Console.WriteLine(item.Type);
            //            Console.WriteLine(item.Value);

            //        }

            //    }
            //    db.SaveChanges();
            //}

        }
    }
}