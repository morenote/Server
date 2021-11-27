using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreNote.Logic.Entity.ConfigFile
{
    /// <summary>
    /// 又拍云设置
    /// </summary>
    public   class UpyunConfig
    {
        public string UpyunSecret { get; set; }
        public string UpyunBucket { get; set; }
        public string UpyunUsername { get; set; }
        public string UpyunPassword { get; set; }
        public string FormApiSecret { get; set; }
    }
}
