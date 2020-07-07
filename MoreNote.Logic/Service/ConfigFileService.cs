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
                        if (RuntimeEnvironment.IsWindows)
                        {
                            path = @"C:\etc\morenote\WebSiteConfig.json";
                        }
                        else
                        {
                            path = "/etc/morenote/WebSiteConfig.json";
                        }
                        if (!File.Exists(path))
                        {
                            throw new IOException($"{path}不存在");
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
        }
    }
}