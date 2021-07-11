using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoreNote.Logic.Entity.ConfigFile;
using MoreNote.Logic.Service.FileService.IMPL;
using MoreNote.Logic.Service.FileStoreService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreNote.Logic.Service.FileService.IMPL.Tests
{
    [TestClass()]
    public class MinIOFileStoreServiceTests
    {
        [TestMethod()]
        public void PresignedGetObjectAsyncTest()
        {
            ConfigFileService configFileService=new ConfigFileService();
            WebSiteConfig webSiteConfig=configFileService.WebConfig;
           
            Console.WriteLine(webSiteConfig.MinIOConfig.Endpoint);
    
            var fileStore=  new MinIOFileStoreService(webSiteConfig.MinIOConfig);
            var result= fileStore.PresignedGetObjectAsync("134b5047e1c21001.png").Result;
            Console.WriteLine(result);
           
        }
    }
}