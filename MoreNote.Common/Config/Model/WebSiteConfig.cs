using System;
using System.Collections.Generic;
using System.Text;

namespace MoreNote.Common.Config.Model
{
    public class WebSiteConfig
    {
        public string connection { get; set; }
        /// <summary>
        /// 随机图片的位置
        /// </summary>
        public string randomImageDir { get; set; }
        public string upyunSecret { get; set; }
        public string upyunBucket { get; set; }
        public string upyunUsername { get; set; }
        public string upyunPassword { get; set; }
        /// <summary>
        /// 随机图片访问保险丝
        /// 当访问数量超过这个保险丝时，对接口进行熔断处理
        /// 避免对整个系统的运行产生破坏
        /// </summary>
        public int randomImageFuseSize { get; set; }
        //随机程度
        public int randomImageSize { get; set; }
    }
}
