﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoreNote.Logic.Entity.ConfigFile;
using MoreNote.Logic.Service.FileService.IMPL;
using MoreNote.Logic.Service.FileStoreService;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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
            var result= fileStore.PresignedGetObjectAsync(webSiteConfig.MinIOConfig.NoteFileBucketName,"13a03c863d021000.png").Result;
            Console.WriteLine(result);
           
        }
        [TestMethod()]
        public void PutObjectAsyncTest()
        {
            ConfigFileService configFileService = new ConfigFileService();
            WebSiteConfig webSiteConfig = configFileService.WebConfig;

            Console.WriteLine(webSiteConfig.MinIOConfig.Endpoint);

            var fileStore = new MinIOFileStoreService(webSiteConfig.MinIOConfig);
            var fileStream= File.OpenRead(@"C:\Users\huany\Pictures\130x130.jpg");
            var len=fileStream.Length;

            fileStore.PutObjectAsync("test", "/my/13a03c863d021000.png", fileStream, len).Wait();
        

        }
    }
}