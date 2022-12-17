using Masuit.MyBlogs.Core.Models.Entity;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreNote.Models.Entity.Leanote.Synchronized
{
    /// <summary>
    /// 提交树
    /// </summary>
    [Table("submit_tree")]
    public class SubmitTree:BaseEntity
    {
        [Column("owner")]
        public long? Owner { get; set; }//拥有者
        [Column("height")]
        public int? Height { get; set; }//当前高度
        [Column("root")]
        public long? Root { get; set; }//树根Id
        [Column("top")]
        public long? Top { get; set; } //树顶Id
    }
}
