﻿using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MoreNote.Controllers.Tests
{
	[TestClass()]
	public class AuthControllerTests
	{
		[TestMethod()]
		public void DoLoginTest()
		{

			//迁移数据

			// using (var db = DataContext.getDataContext())
			// {
			//     var noteFiles = dataContext.File.ToList();
			//     foreach (var noteFile in noteFiles)
			//     {
			//         // /www/upload/10fc1cfeba021000/images/2020_02/110e684d86421000.jpeg
			//         // /user/10fc1cfeba021000/upload/images/2020_02/110e684d86421000.jpeg
			//         if (noteFile.Path.Substring(0,4).Equals("/www"))
			//         {
			//             noteFile.Path = noteFile.Path.Replace(@"/www/upload", @"/user");
			//             noteFile.Path = noteFile.Path.Replace(@"/images/", @"/upload/images/");
			//
			//         }
			//     }
			//     dataContext.SaveChanges();
			// }

			//using (var db= DataContext.getDataContext())
			//{
			//    User user = dataContext.User.Where(b => b.Username.Equals("hyfree")).FirstOrDefault();
			//    Console.WriteLine(user.Email);
			//    var authorizations= user.Jurisdiction;
			//    if (authorizations==null)
			//    {
			//        //Assert.Fail("authorizations==null");
			//        authorizations = new List<Authorization>();
			//        authorizations.Add(new Authorization(SnowFlake_Net.NextId(), "user", "12"));


			//    }
			//    if (authorizations.Any())
			//    {
			//        foreach (var item in authorizations)
			//        {
			//            Console.WriteLine(item.Type);
			//            Console.WriteLine(item.Value);

			//        }

			//    }
			//    dataContext.SaveChanges();
			//}

		}
	}
}