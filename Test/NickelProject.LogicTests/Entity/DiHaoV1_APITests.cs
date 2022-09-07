using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoreNote.Common.Utils;
using MoreNote.Logic.Entity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace MoreNote.Logic.Entity.Tests
{
    [TestClass()]
    public class DiHaoV1_APITests
    {
        [TestMethod()]
        public void TestTest()
        {
            APPStoreInfo diHaoV1_API = new APPStoreInfo()
            {
                resp_data = new Resp_Data()
                {
                    app_list = new AppInfo[]
                    {
                     new AppInfo()
                     {
                        Id =111,
                        appautor = "appid",
                        appdetail = "appid",
                        appname = "appid",
                        apppackage = "appid",
                        appdownurl = "appid",
                        applogourl = "appid",
                        appversion = "appid",
                        imglist = new string[] { "","" },
                        appsize = "appid"
                     }
                        
                    }
                }
            };
            string json = JsonSerializer.Serialize(diHaoV1_API, MyJsonConvert.GetLeanoteOptions());
            Console.WriteLine(json);
        }
    }
}