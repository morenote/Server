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
        /// 密钥 用于敏感操作
        /// 每次软件启动后，会使用随机密钥填充这个值
        /// 在进行某些敏感操作的时候，系统会询问你的密钥 比如重置admin管理员的密码
        /// 但是务必注意的是：你的密码重置后，服务器端保存的加密数据是无法恢复
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
        /// <summary>
        /// 网站紧急维护模式
        /// 当需要重置管理员密码的时候，需要打开紧急维护模式
        /// </summary>
        public bool MaintenanceMode { get;set;}=false;
        /// <summary>
        /// 密码加密算法
        /// </summary>
        public string HashAlgorithm { get;set;}= "argon2";
        /// <summary>
        /// 密码加密时的迭代次数
        /// 迭代次数越大，计算越困难
        /// </summary>
        public int Pwd_Cost { get;set;}= 8;
        /// <summary>
        /// 密码加密时的cpu线程限制 仅适用于Argon2id
        /// cpu核心x2
        /// </summary>
        public int PasswordStoreDegreeOfParallelism=8;
        /// <summary>
        /// 密码加密时的内存限制 仅适用于Argon2id
        /// 内存越大，计算越困难
        /// </summary>
        public int PasswordStoreMemorySize=1024*2;

    }
}
