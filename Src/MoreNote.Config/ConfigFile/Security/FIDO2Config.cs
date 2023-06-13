using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreNote.Models.Entity.ConfigFile
{
    /// <summary>
    /// Fido2配置
    /// https://github.com/passwordless-lib/fido2-net-lib/blob/master/Src/Fido2.Models/Fido2Configuration.cs
    /// </summary>
    public class FIDO2Config
    {
       /// <summary>
       /// 是否启用
       /// </summary>
        public  bool IsEnable { get; set; }=false;
        /// <summary>
        /// 服务器域名
        /// </summary>
        public string ServerDomain{get;set;}
        /// <summary>
        /// 服务器域名名称
        /// </summary>
        public string ServerName{get;set;}
        /// <summary>
        /// 服务器域名地址
        /// 例如https://localhost:5001
        /// </summary>
        public string[] Origin{get;set;}
        /// <summary>
        /// 允许时间戳漂移（毫秒）
        /// </summary>
        public int TimestampDriftTolerance{get;set;}=300000;
        /// <summary>
        /// 缓存
        /// </summary>
        public string MDSCacheDirPath{get;set;}=string.Empty;
    }
}
