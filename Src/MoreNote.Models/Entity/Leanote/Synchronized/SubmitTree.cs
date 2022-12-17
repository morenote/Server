using Masuit.MyBlogs.Core.Models.Entity;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreNote.Models.Entity.Leanote.Synchronized
{
    /// <summary>
    /// 提交树
    /// </summary>
    public class SubmitTree:BaseEntity
    {

        public long Owner { get; set; }//拥有者
        public int Height { get; set; }//当前高度
        public long Root { get; set; }//树根Id
        public long Top { get; set; } //树顶Id
    }
}
