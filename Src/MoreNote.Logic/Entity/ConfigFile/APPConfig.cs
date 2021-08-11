using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreNote.Logic.Entity.ConfigFile
{
   public class APPConfig
    {
        /// <summary>
        /// APP的名字 也就是你网站的名字
        /// </summary>
        public string APPName{get;set;}="";
        /// <summary>
        /// 默认的语言
        /// </summary>
        public string DefaultLanguage="en-us";
        /// <summary>
        /// 网站域名
        /// </summary>
        public string SiteUrl { get;set;}="/";
        public string BlogUrl { get;set;}="/blog";
        public string LeaUrl { get;set;}="";
        public string NoteUrl { get;set;}="/note/note";


        /// <summary>
        /// 使用什么类型的数据库
        /// sqlserver mysql mongdb sqlite
        /// </summary>
        public string DB { get;set;}= "postgresql";
        /// <summary>
        /// 开发模式
        /// 另外可以通过设置环境变量设置
        /// </summary>
        public bool Dev { get;set;}
        /// <summary>
        /// 用户上传/附件/主题文件夹
        /// </summary>
        public string FileFolder { get;set;}
    }
}
