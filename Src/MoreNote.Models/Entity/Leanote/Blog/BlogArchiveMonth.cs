using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreNote.Models.Entity.Leanote.Blog
{
    // 归档 数据容器 不需要数据库储存
    public class BlogArchiveMonth
    {
        public int Month { get; set; }
        public BlogPost[] Posts { get; set; }
    }

}
