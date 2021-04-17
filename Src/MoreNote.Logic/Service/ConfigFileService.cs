using MoreNote.Common.Utils;
using MoreNote.Logic.Entity.ConfigFile;

using System.IO;
using System.Text.Json;

namespace MoreNote.Logic.Service
{
    /// <summary>
    /// 读取配置文件的类
    /// </summary>
    public class ConfigFileService
    {
        private string path = null;

        private WebSiteConfig Config { get; set; }

        // 定义一个标识确保线程同步
        private static readonly object locker = new object();

        public ConfigFileService()
        {
            if (Config == null)
            {
                Config = GetWebConfig();
            }
        }

        public static string GetConfigPath()
        {
            if (RuntimeEnvironment.IsWindows)
            {
                return @"C:\morenote\config.json";
            }
            else
            {
                return "/morenote/config.json";
            }
        }

        /// <summary>
        /// 初始化模板
        /// </summary>
        private void InitTemplateConfig()
        {
            if (RuntimeEnvironment.IsWindows)
            {
                if (!Directory.Exists(@"C:\morenote"))
                {
                    Directory.CreateDirectory(@"C:\morenote");
                }
            }
            else
            {
                if (!Directory.Exists(@"/morenote"))
                {
                    Directory.CreateDirectory(@"/morenote");
                }
            }
            WebSiteConfig webSiteConfig = new WebSiteConfig();
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,//优质打印 无压缩
            };
            string json = System.Text.Json.JsonSerializer.Serialize(webSiteConfig, options);
            File.Create(path).Close();
            File.WriteAllText(path, json);
        }

        public WebSiteConfig GetWebConfig()
        {
            if (Config == null)
            {
                lock (locker)
                {
                    if (Config == null)
                    {
                        path = GetConfigPath();
                        if (!File.Exists(path))
                        {
                            InitTemplateConfig();
                        }
                        string json = File.ReadAllText(path);
                        Config = System.Text.Json.JsonSerializer.Deserialize<WebSiteConfig>(json);
                    }
                }
            }

            return Config;
        }

        public void Save()
        {
            lock (locker)
            {
                if (Config == null)
                {
                    return;
                }
                string json = System.Text.Json.JsonSerializer.Serialize(Config);
                File.WriteAllText(path, json);
            }
        }

        public void Save(WebSiteConfig tempConfig, string onePath)
        {
            lock (locker)
            {
                if (tempConfig == null)
                {
                    return;
                }
                string json = System.Text.Json.JsonSerializer.Serialize(tempConfig);
                File.WriteAllText(onePath, json);
                Config = tempConfig;
            }
        }
    }
}