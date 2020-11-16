using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreNote.Logic.Entity.ConfigFile
{
    /// <summary>
    /// 网络爬虫配置
    /// </summary>
    public class SpidersConfig
    {
        /// <summary>
        /// 是否启用爬虫服务
        /// </summary>
        public bool CrawlerWorker { get; set; }
        /// <summary>
        /// 爬虫抓取速度
        /// </summary>
        public int Reptile_Delay_Second { get; set; }

  
    }
}
