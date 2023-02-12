using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreNote.Logic.Entity.ConfigFile
{
    /// <summary>
    /// 文件配置
    /// </summary>
  public  class FileStoreConfig
    {
        /// <summary>
        /// 主要文件夹
        /// </summary>
        public string MainFolder { get;set;}="/morenote";
        /// <summary>
        /// 静态文件单独一个请求域名
        /// </summary>
        public string StaticDomain { get;set;}=null;


        /// <summary>
        /// 使用什么作为存储服务
        /// 目前支持 minio upyun disk
        /// 默认使用minio
        /// </summary>
        public string FileStorage { get; set; }="minio";
        /// <summary>
        /// 存储服务生成下载凭证的失效时间 秒
        /// </summary>
        public int BrowserDownloadExpiresInt { get; set; }=3600;  
        /// <summary>
        /// 存储服务生成上传凭证的失效时间 秒
        /// </summary>
        public int BrowserUploadExpiresInt { get; set; }=3600;
        /// <summary>
        /// 用户头像最大大小MB
        /// </summary>
        public int UploadAvatarMaxSizeMB { get; set; } = 20;
        /// <summary>
        /// 用户BlogLogo限制MB
        /// </summary>
        public int UploadBlogLogoMaxSizeMB { get; set; } = 20;
        /// <summary>
        /// 用户图片限制MB
        /// </summary>
        public int UploadImageMaxSizeMB { get; set; } = 20;
        /// <summary>
        /// 用户附件限制MB
        /// </summary>
        public int UploadAttachMaxSizeMB { get;set;}=500;
        /// <summary>
        /// 临时文件夹(临时文件和缓存文件)
        /// </summary>
        public string TempFolder { get;set;}= "/var/tmp";
    }
}
