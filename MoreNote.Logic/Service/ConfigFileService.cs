using MoreNote.Logic.Entity.ConfigFile;

using System;
using System.Collections.Generic;
using System.Text;
using MoreNote.Common.Util;
using System.IO;

namespace MoreNote.Logic.Service
{
   public class ConfigFileService
    {
        private static WebSiteConfig config;
        public static WebSiteConfig GetWebConfig()
        {
            if (config != null)
            {
                return config;
            }
            if (RuntimeEnvironment.Islinux)
            {
                string path = "/etc/morenote/PostgreSQLConfig.json";
                string json = File.ReadAllText(path);
                WebSiteConfig postgreSQLConfig = System.Text.Json.JsonSerializer.Deserialize<WebSiteConfig>(json);
                config = postgreSQLConfig;
                return postgreSQLConfig;

            }
            else
            {
                string path = @"C:\etc\morenote\PostgreSQLConfig.json";
                string json = File.ReadAllText(path);
                WebSiteConfig postgreSQLConfig = System.Text.Json.JsonSerializer.Deserialize<WebSiteConfig>(json);
                config = postgreSQLConfig;
                return postgreSQLConfig;
            }

        }
    }
}
