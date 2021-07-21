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
        public string SaveFolder { get;set;}=null;
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
        public int UploadAvatarMaxSizeMB { get; set; } = 10;
        public long UploadBlogLogoMaxSizeMB { get; set; } = 10;
        public long UploadImageMaxSizeMB { get; set; } = 20;
    }
}
