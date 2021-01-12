using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoreNote.Logic.Service;
using System;
using System.Collections.Generic;
using System.Text;

namespace MoreNote.Logic.Service.Tests
{
    [TestClass()]
    public class ConfigServiceTests
    {
        ConfigFileService configFileService=new ConfigFileService();
      
        [TestMethod()]
        public void GetConfigServiceTest()
        {

        }

        [TestMethod()]
        public void SaveTest()
        {
            ConfigService configService = new ConfigService();
            if (configService.emailConfig == null)
            {
                configService.emailConfig = new ConfigService.EmailConfig();

            }
            configService.emailConfig.EnableSsl = true;
            configService.emailConfig.Host = "123";
            configService.emailConfig.UseDefaultCredentials = true;
            configService.emailConfig.Port = 456;
            configService.emailConfig.userName = "username";
            configService.emailConfig.password = "password";
            configFileService.Save();
           // Assert.Fail();
        }
    }
}