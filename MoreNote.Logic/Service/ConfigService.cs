using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;

namespace MoreNote.Logic.Service
{
    public class ConfigService
    {
        private static ConfigService _configService;
        private static object _lockObject = new object();
        private const string path = @"Config\config.json";
        public EmailConfig emailConfig;

        public struct GlobalConfigService
        {
            long adminUserId;
            string siteUrl;
            string adminUserName;
            //全局
            Dictionary<string,string> GlobalAllConfigs;
            Dictionary<string,string> GlobalStringConfigs;
            Dictionary<string,string[]> GlobalArrayConfigs;
            Dictionary<string,string> GlobalMapConfigs;
            Dictionary<string,string> GlobalArrMapConfigs;
        }
        // appStart时 将全局的配置从数据库中得到作为全局
        public static bool InitGlobalConfigs()
        {
            throw new Exception();
        }
        public static string GetSiteUrl()
        {
            throw new Exception();
        }
        public static bool updateGlobalConfig(long userid,string key,string value)
        {
            throw new Exception();
        }
        public static bool UpdateGlobalStringConfig(long userId,string key,string value)
        {
            throw new Exception();
        }
        //获取全局配置, 博客平台使用
        public static string GetGlobalStringConfig(string key)
        {
            throw new Exception();
        }
        public static string[] GetGlobalArrayConfig(string key)
        {
            throw new Exception();
        }
        public static HashSet<string> GetGlobalMapConfig(string key)
        {
            throw new Exception();
        }
        public static HashSet<string>[] GetGlobalArrMapConfig(string key)
        {
            throw new Exception();
        }
        public bool IsOpenRegister()
        {
            throw new Exception();
        }
        //-------
        // 修改共享笔记的配置
        public static bool UpdateShareNoteConfig(long registerSharedUserId,int[] registerSharedNotebookPerms,int[] registerSharedNotePerms,long[] registerSharedNotebookIds,long[] registerSharedNoteIds,long[] registerCopyNoteIds)
        {
            throw new Exception();
        }
        public static bool AddBackup(string path,string remark)
        {
            throw new Exception();
        }
        public static string getBackupDirname()
        {
            throw new Exception();
        }
        public static bool Backup(string remark)
        {
            throw new Exception();
        }
        public static bool Restore(string createTime)
        {
            throw new Exception();
        }
        public static bool DeleteBackup(string createdTime)
        {
            throw new Exception();
        }
        public static bool UpdateBackupRemark(string createdTime,string remark)
        {
            throw new Exception();
        }
        public static Dictionary<string,string> GetBackup(string createdTime)
        {
            throw new Exception();
        }
        //--------------
        // sub domain
        string defaultDomain;
        string schema= @"http://";
        string port;
        public static void init()
        {
            throw new Exception();
        }
        public static string GetSchema()
        {
            throw  new Exception();
        }
        // 默认
        public static string GetDefaultDomain()
        {
            throw new Exception();
        }
        // note 
        public static string GetNoteDomain()
        {
            throw new Exception();
        }
        public static string GetNoteUrl()
        {
            throw new Exception();
        }
        //blog
        public static string GetBlogDomain()
        {
            throw new Exception();
        }
        public static string GetBlogUrl()
        {
            throw 
                 new Exception();
        }
        //lea
        public static string GetLeaDomain()
        {
            throw new Exception();
        }
        public static string GetLeaUrl()
        {
            throw new Exception();
        }
        public static string GetUserUrl(string domain)
        {
            throw 
                 new Exception();
        }
        public static string GetUserSubUrl(string subDomain)
        {
            throw 
                 new Exception();
        }
        // 是否允许自定义域名
        public bool AllowCustomDomain()
        {
            //默认 应该是总是允许的
            //morenote 专为单租户场景设置的
            //所以 总是假设用户拥有自定义域名的权利
            //同时，且仅有一个域名
            //不支持多域名
            throw new Exception();

        }
        public static bool IsGoodCustomDomain(string domain)
        {
            throw new Exception();
        }
        public static bool IsGoodSubDomain(string domain)
        {
            throw new Exception();
        }
        public static long GetUploadSize(string key)
        {
            throw  new Exception();
        }
        public static long GetInt64(string key)
        {
            throw new Exception();
        }
        public static int GetInt32(string key)
        {
            throw new Exception();
        }
        public static Dictionary<string ,long> GetUploadSizeLimit()
        {
            throw new Exception();
        }
        // 为用户得到全局的配置
        // NoteController调用
        public static Dictionary<string,object> GetGlobalConfigForUser()
        {
            throw new Exception();
        }
        //主页是否是管理员的博客页
        public static bool HomePageIsAdminsBlog()
        {
            throw new Exception();
        }
        public string GetVersion()
        {
            return "0.0.1";
        }
        public string GetLeanoteVersion()
        {
            //morenote 0.0.1版本是基于leanote 2.6.1版本的.net core发行版
            //0.0.1计划实现2.6.1的全部API的部分
            return "2.6.1";
        }




        public class EmailConfig
        {
            public String Host { get; set; }
            public bool EnableSsl { get; set; }
            public int Port { get; set; }
            public bool UseDefaultCredentials { get; set; }
            public string userName { get; set; }
            public string password { get; set; }
        }
        public static ConfigService GetConfigService()
        {
            lock (_lockObject)
            {
                if (_configService != null)
                {
                    return _configService;
                }
                else
                {
                    try
                    {
                        using (StreamReader streamReader = new StreamReader(path, Encoding.UTF8))
                        {
                            string str = streamReader.ReadToEnd();
                            _configService = JsonSerializer.Deserialize<ConfigService>(str);
                        }
                        return _configService;
                    }
                    catch (Exception e)
                    {
                        return null;

                    }


                }

            }


        }
        public static void Save(ConfigService configService)
        {
            lock (_lockObject)
            {
                using (StreamWriter streamWriter = new StreamWriter(path, false, Encoding.UTF8))
                {
                    var options = new JsonSerializerOptions
                    {
                        WriteIndented = true
                    };
                    string a = JsonSerializer.Serialize(configService);
                    streamWriter.WriteLine(a);
                    streamWriter.Flush();
                }
                _configService = configService;

            }

        }




    }
}
