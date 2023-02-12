using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreNote.Logic.Service.BlogBuilder
{
    /// <summary>
    /// 博客生成器管道处理器接口
    /// </summary>
    public interface  BlogBuilderPipeInterface
    {
        public  string BlogBuilderDataProcessing(string Content);
      
    }
}
