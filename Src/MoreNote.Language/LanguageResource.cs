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
        private  Dictionary<string ,string> album = null;
        private  Dictionary<string ,string> blog = null;
        private  Dictionary<string ,string> markdown = null;
        private  Dictionary<string ,string> member = null;
        private  Dictionary<string ,string> msg = null;
        private  Dictionary<string ,string> note = null;
        private  Dictionary<string ,string> tinymce_editor = null;


        private static Dictionary<string,Dictionary<string,string>> Values=null;
        private static char dsc=Path.DirectorySeparatorChar;


        // 定义一个标识确保线程同步
        private static readonly object locker = new object();

        /// <summary>
        /// 区域 地区
        /// </summary>
        private string locale;
        public LanguageResource(string locale)
        {
            this.locale=locale;

        }

        public  Dictionary<string, string> GetAlbum()
        {
            if (album == null)
            {
                lock (locker)
                {
                    // 如果类的实例不存在则创建，否则直接返回
                    if (album == null)
                    {
                        album = ReadConf("album.conf");
                    }
                }
            }
            return album;
        }
        public  Dictionary<string, string> GetBlog()
        {
            if (blog == null)
            {
                lock (locker)
                {
                    // 如果类的实例不存在则创建，否则直接返回
                    if (blog == null)
                    {
                        blog = ReadConf("blog.conf");
                    }
                }
            }
            return blog;
        }

        public  Dictionary<string, string> GetMarkdown()
        {
            if (markdown == null)
            {
                lock (locker)
                {
                    // 如果类的实例不存在则创建，否则直接返回
                    if (markdown == null)
                    {
                        markdown = ReadConf("markdown.conf");
                    }
                }
            }
            return markdown;
        }
        public  Dictionary<string, string> GetMember()
        {
            if (member == null)
            {
                lock (locker)
                {
                    // 如果类的实例不存在则创建，否则直接返回
                    if (member == null)
                    {
                        member = ReadConf("member.conf");
                    }
                }
            }
            return member;
        }
        public  Dictionary<string, string> GetMsg()
        {
            if (msg == null)
            {
                lock (locker)
                {
                    // 如果类的实例不存在则创建，否则直接返回
                    if (msg == null)
                    {
                        msg = ReadConf("msg.conf");
                    }
                }
            }
            return msg;
        }
        public  Dictionary<string, string> GetNote()
        {
            if (note == null)
            {
                lock (locker)
                {
                    // 如果类的实例不存在则创建，否则直接返回
                    if (note == null)
                    {
                        note = ReadConf("note.conf");
                    }
                }
            }
            return note;
        }
        public  Dictionary<string, string> GetTinymce_editor()
        {
            if (tinymce_editor == null)
            {
                lock (locker)
                {
                    // 如果类的实例不存在则创建，否则直接返回
                    if (tinymce_editor == null)
                    {
                        tinymce_editor = ReadConf("tinymce_editor.conf");
                    }
                }
            }
            return tinymce_editor;
        }
        private  Dictionary<String,string> ReadConf(string name)
        {
           // string path=Environment.CurrentDirectory+$"{dsc}Value{dsc}zh-cn{dsc}{name}";//工作目录{
            string path= AppDomain.CurrentDomain.SetupInformation.ApplicationBase+$"{dsc}Value{dsc}{locale}{dsc}{name}";//工作目录

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
