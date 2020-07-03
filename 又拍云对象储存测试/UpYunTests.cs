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
            //WebSiteConfig postgreSQLConfig = ConfigManager.GetWebConfig();
            //UpYun upyun = new UpYun("bucket", "username", "password");
            //upyun.secret = postgreSQLConfig.upyunSecret; ;
            //Int32 unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
            //unixTimestamp += 120;
            //string _upt = upyun.CreatToken(unixTimestamp.ToString(), upyun.secret, $"/upload/debbb8740a597ca554d194818af6e560/270a9d65cfb903fb89cb21c78bd6bad106b543ca.jpg");
            //Console.WriteLine($"https://upyun.morenote.top/upload/debbb8740a597ca554d194818af6e560/270a9d65cfb903fb89cb21c78bd6bad106b543ca.jpg?_upt={_upt}");
        }

        [TestMethod()]
        public void UpYunTest()
        {
            //WebSiteConfig postgreSQLConfig = ConfigManager.GetWebConfig();
            //UpYun upyun = new UpYun(postgreSQLConfig.upyunBucket, postgreSQLConfig.upyunUsername, postgreSQLConfig.upyunPassword);
            //ArrayList str = upyun.readDir("/");
            //foreach (var item in str)
            //{
            //    Console.WriteLine(item);
            //}

        }
    }
}