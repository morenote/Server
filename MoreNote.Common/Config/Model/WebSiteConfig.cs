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
        /// <summary>
        /// 是否启动token防盗链
        /// </summary>
        public bool token_anti_theft_chain { get; set; }
        /// <summary>
        /// 爬虫抓取速度
        /// </summary>
        public int Reptile_Delay_Second { get; set; }
        //商户ID
        public String PayJS_MCHID { get; set; }
        //密钥
        public String PayJS_Key { get; set; }

    }
}
