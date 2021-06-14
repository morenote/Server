using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreNote.Logic.Entity.ConfigFile
{
    public class SecurityConfig
    {
        /// <summary>
        /// 秘钥值 用于敏感操作
        /// 软件启动后，会读取这个口令
        /// Secret为空的话，就无法执行操作 默认为空
        /// 在进行某些敏感操作的时候，系统会询问你的秘钥值 比如重置admin管理员的密码
        /// 你的密码重置后，在服务器端保存的加密数据无法恢复
        /// </summary>
        public string Secret{get;set;}
        /// <summary>
        /// 是否允许第三方注册
        /// 邀请注册 不受限制
        /// </summary>
        public bool OpenRegister{get;set;}=false;
        /// <summary>
        /// 是否允许开启demo体验账号
        /// demo账号允许匿名未知来宾使用你的服务 
        /// </summary>
        public bool OpenDemo{get;set;}=false;
        /// <summary>
        /// 允许发送实时数据和诊断数据到我们的服务器
        /// 以便我们进行故障遥测和分析
        /// 默认=false
        /// </summary>
        public bool ShareYourData{get;set;}=false;
        /// <summary>
        /// 指定某个用户成为超级管理员
        /// 程序首次初始化时 admin是超级管理员
        /// </summary>
        public string AdminUsername{ get;set;}="admin";
        /// <summary>
        /// 将log放置在哪里
        /// </summary>
        public string LogFolder{ get;set;}
        /// <summary>
        /// Session有效期
        /// 也就是保持登录的有效期
        /// </summary>
        public int SessionExpires { get;set;}
    }
}
