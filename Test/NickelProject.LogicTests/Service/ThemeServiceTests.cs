using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoreNote.Common.Utils;
using MoreNote.Logic.Entity;
using MoreNote.Logic.Service;
using System;
using System.Collections.Generic;
using System.Text;

namespace MoreNote.Logic.Service.Tests
{
    [TestClass()]
    public class ThemeServiceTests
    {
        [TestMethod()]
        public void GetURLsTest()
        {

            //Assert.Fail();
        }

        [TestMethod()]
        public void InsertThemeTest()
        {
            //Theme theme = new Theme();
            //theme.ThemeId = SnowFlake_Net.GenerateSnowFlakeID();
            //// theme.FriendLinksArray =new FriendLinks[]{ new FriendLinks() {FriendLinksId= SnowFlake_Net.GenerateSnowFlakeID() ,Title="A",Url="A"} };
            //ThemeService.InsertTheme(theme);
            //Assert.Fail();
        }

        [TestMethod()]
        public void InsertURLTest()
        {
            FriendLinks friendLinks = new FriendLinks();
            friendLinks.FriendLinksId = SnowFlakeNetService.GenerateSnowFlakeID();
            friendLinks.ThemeId = SnowFlakeNetService.GenerateSnowFlakeID();
            friendLinks.Title = "标题1";
            friendLinks.Title = "标题2";
            //Assert.Fail();
        }
    }
}