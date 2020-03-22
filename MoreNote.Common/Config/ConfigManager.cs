using System.IO;
using MoreNote.Common.Config.Model;
using MoreNote.Common.Util;

namespace MoreNote.Common.Config
{
    //配置管理器
    public class ConfigManager
    {
        public static PostgreSQLConfig GetPostgreSQLConfig()
        {
            if (RuntimeEnvironment.Islinux)
            {
                string path = "/etc/morenote/PostgreSQLConfig.json";
                string json = File.ReadAllText(path);
                PostgreSQLConfig postgreSQLConfig = System.Text.Json.JsonSerializer.Deserialize<PostgreSQLConfig>(json);
                return postgreSQLConfig;

            }
            else
            {
                string path = @"C:\etc\morenote\PostgreSQLConfig.json";
                string json = File.ReadAllText(path);
                PostgreSQLConfig postgreSQLConfig = System.Text.Json.JsonSerializer.Deserialize<PostgreSQLConfig>(json);
                return postgreSQLConfig;
            }
        }
    }
}
