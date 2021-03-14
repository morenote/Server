using MoreNote.Language.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MoreNote.Value
{
    public  class LanguageResource
    {
        private static Dictionary<string ,string> album = null;
        private static Dictionary<string ,string> blog = null;
        private static Dictionary<string ,string> markdown = null;
        private static Dictionary<string ,string> member = null;
        private static Dictionary<string ,string> msg = null;
        private static Dictionary<string ,string> note = null;
        private static Dictionary<string ,string> tinymce_editor = null;


        private static Dictionary<string,Dictionary<string,string>> Values=null;
        private static char dsc=Path.DirectorySeparatorChar;


        // 定义一个标识确保线程同步
        private static readonly object locker = new object();

        public static Dictionary<string, string> GetAlbum()
        {
            if (album == null)
            {
                lock (locker)
                {
                    // 如果类的实例不存在则创建，否则直接返回
                    if (album == null)
                    {
                        album = readConf("album.conf", LanguageEnum.ZH_CN);
                    }
                }
            }
            return album;
        }
        public static Dictionary<string, string> GetBlog()
        {
            if (blog == null)
            {
                lock (locker)
                {
                    // 如果类的实例不存在则创建，否则直接返回
                    if (blog == null)
                    {
                        blog = readConf("blog.conf", LanguageEnum.ZH_CN);
                    }
                }
            }
            return blog;
        }

        public static Dictionary<string, string> GetMarkdown()
        {
            if (markdown == null)
            {
                lock (locker)
                {
                    // 如果类的实例不存在则创建，否则直接返回
                    if (markdown == null)
                    {
                        markdown = readConf("markdown.conf", LanguageEnum.ZH_CN);
                    }
                }
            }
            return markdown;
        }
        public static Dictionary<string, string> GetMember()
        {
            if (member == null)
            {
                lock (locker)
                {
                    // 如果类的实例不存在则创建，否则直接返回
                    if (member == null)
                    {
                        member = readConf("member.conf", LanguageEnum.ZH_CN);
                    }
                }
            }
            return member;
        }
        public static Dictionary<string, string> GetMsg()
        {
            if (msg == null)
            {
                lock (locker)
                {
                    // 如果类的实例不存在则创建，否则直接返回
                    if (msg == null)
                    {
                        msg = readConf("msg.conf", LanguageEnum.ZH_CN);
                    }
                }
            }
            return msg;
        }
        public static Dictionary<string, string> GetNote()
        {
            if (note == null)
            {
                lock (locker)
                {
                    // 如果类的实例不存在则创建，否则直接返回
                    if (note == null)
                    {
                        note = readConf("note.conf", LanguageEnum.ZH_CN);
                    }
                }
            }
            return note;
        }
        public static Dictionary<string, string> GetTinymce_editor()
        {
            if (tinymce_editor == null)
            {
                lock (locker)
                {
                    // 如果类的实例不存在则创建，否则直接返回
                    if (tinymce_editor == null)
                    {
                        tinymce_editor = readConf("tinymce_editor.conf", LanguageEnum.ZH_CN);
                    }
                }
            }
            return tinymce_editor;
        }
        private static Dictionary<String,string> readConf(string name, LanguageEnum languageType)
        {
           // string path=Environment.CurrentDirectory+$"{dsc}Value{dsc}zh-cn{dsc}{name}";//工作目录{
            string path= AppDomain.CurrentDomain.SetupInformation.ApplicationBase+$"{dsc}Value{dsc}zh-cn{dsc}{name}";//工作目录

            string[] vs=   System.IO.File.ReadAllLines(path);
            Dictionary<string, string> dic = new Dictionary<string, string>(100);
            foreach (string str in vs)
            {
                if(!String.IsNullOrEmpty(str))
                if (str[0].Equals("#"))
                {
                    continue;
                }
                else
                {
                        Console.WriteLine(str);
                        int i = str.IndexOf("=");
                        if (i > -1)
                        {
                            string key = str.Substring(0, i);
                            if (!dic.ContainsKey(key))
                            {
                                dic.Add(key.Trim(), str.Substring(i + 1).Trim());
                            }
                        }
                }
            }
            return dic;
        }
      


    }
}
