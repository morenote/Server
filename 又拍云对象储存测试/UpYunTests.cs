using Microsoft.VisualStudio.TestTools.UnitTesting;
using UpYunLibrary;
using System;
using System.Collections.Generic;
using System.Text;
using MoreNote.Common.Config;
using MoreNote.Common.Config.Model;
using System.Collections;

namespace UpYunLibrary.Tests
{
    [TestClass()]
    public class UpYunTests
    {
        [TestMethod()]
        public void CreatTokenTest()
        {
            WebSiteConfig postgreSQLConfig = ConfigManager.GetPostgreSQLConfig();
            UpYun upyun = new UpYun("bucket", "username", "password");
            upyun.secret = postgreSQLConfig.upyunSecret; ;
            Int32 unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
            unixTimestamp += 120;
            string _upt = upyun.CreatToken(unixTimestamp.ToString(), upyun.secret, "/74956136_p0.jpg");
            Console.WriteLine($"https://upyun.morenote.top/74956136_p0.jpg?_upt={_upt}");
        }

        [TestMethod()]
        public void UpYunTest()
        {
            WebSiteConfig postgreSQLConfig = ConfigManager.GetPostgreSQLConfig();
            UpYun upyun = new UpYun(postgreSQLConfig.upyunBucket, postgreSQLConfig.upyunUsername, postgreSQLConfig.upyunPassword);
            ArrayList str = upyun.readDir("/");
            foreach (var item in str)
            {
                Console.WriteLine(item);
            }

        }
    }
}