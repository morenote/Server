using MoreNote.Common.Util;
using MoreNote.Logic.Entity.ConfigFile;

using System.IO;

namespace MoreNote.Logic.Service
{
    /// <summary>
    /// 读取配置文件的类
    /// </summary>
    public class ConfigFileService
    {
        private static string path = null;

        private static WebSiteConfig config;

        // 定义一个标识确保线程同步
        private static readonly object locker = new object();

        public static string GetConfigPath()
        {
            if (RuntimeEnvironment.IsWindows)
            {
                return @"C:\morenote\WebSiteConfig.json";
            }
            else
            {
                return "/morenote/WebSiteConfig.json";
            }
        }

        private static void InitWindowConfigDirectory()
        {
            if (!Directory.Exists(@"C:\morenote"))
            {
                Directory.CreateDirectory(@"C:\morenote");
            }
        }

        private static void InitLinuxConfigDirectory()
        {
            if (!Directory.Exists(@"/morenote"))
            {
                Directory.CreateDirectory(@"/morenote");
            }
        }

        private static void InitConfig()
        {
            if (RuntimeEnvironment.IsWindows)
            {
                InitWindowConfigDirectory();
            }
            else
            {
                InitLinuxConfigDirectory();
            }
            WebSiteConfig webSiteConfig = new WebSiteConfig();
            string json = System.Text.Json.JsonSerializer.Serialize(webSiteConfig);
            File.Create(path).Close();
            File.WriteAllText(path, json);
         
        }

        private ConfigFileService()
        {
            if (config == null)
            {
                config = GetWebConfig();
            }
        }
        public static WebSiteConfig GetWebConfig()
        {
            if (config == null)
            {
                lock (locker)
                {
                    if (config == null)
                    {
                        path = GetConfigPath();
                        if (!File.Exists(path))
                        {
                            InitConfig();
                        }
                        string json = File.ReadAllText(path);
                        config = System.Text.Json.JsonSerializer.Deserialize<WebSiteConfig>(json);
                    }
                }
            }

            return config;
        }

        public static void Save()
        {
            if (config == null)
            {
                throw new System.Exception("config==null,无法将config持久化保存。");
            }
            string json = System.Text.Json.JsonSerializer.Serialize(config);
            File.WriteAllText(path, json);
        }

        public static void Save(WebSiteConfig tempConfig, string onePath)
        {
            if (tempConfig == null)
            {
                throw new System.Exception("config==null,无法将config持久化保存。");
            }
            string json = System.Text.Json.JsonSerializer.Serialize(tempConfig);
            File.WriteAllText(onePath, json);
            config=tempConfig;
        }
    }
}