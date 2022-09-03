using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreNote.Config.ConfigFile
{
    public class BlogConfig
    {
        /// <summary>
        /// 博客生成器默认工作目录
        /// </summary>
        public string BlogBuilderWorkingDirectory { get; set; }= "/morenote/BlogBuilder/output/";
        public string BlogBuilderVuePressTemplate { get; set; } = "/morenote/BlogBuilder/template/vuepress/";
    }
}
