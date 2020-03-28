using System;
using System.Collections.Generic;
using System.Text;

namespace MoreNote.Common.Config.Model
{
    public class PostgreSQLConfig
    {
        public string connection { get; set; }
        /// <summary>
        /// 随机图片的位置
        /// </summary>
        public string randomImageDir { get; set; }
    }
}
