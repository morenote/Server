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
        /// 软件启动后，总是会刷新这个秘钥值
        /// 在进行某些敏感操作的时候，系统会询问你的秘钥值 比如重置admin管理员的密码
        /// 因为服务器端不会保存你的密码
        /// 所以如果你遗忘你的密码 那些受加密算法保护的笔记数据将无法被解密 全部丢失
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
        public string AdminUsername{ get;set;}="admin";

    }
}
