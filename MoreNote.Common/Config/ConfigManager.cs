using System.IO;
using MoreNote.Common.Config.Model;
using MoreNote.Common.Util;

namespace MoreNote.Common.Config
{

    //配置管理器
    public class ConfigManager
    {
        private  static WebSiteConfig config;

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
