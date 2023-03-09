using Morenote.Models.Models.Entity;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreNote.Models.Entity.Leanote.Blog
{
    //博客统计信息
    [Table("blog_stat")]
    public class BlogStat : BaseEntity
    {

        [Column("node_id")]
        public long? NodeId { get; set; }
        [Column("read_num")]
        public int ReadNum { get; set; } // 阅读次数 
        [Column("like_num")]
        public int LikeNum { get; set; } // 点赞次数 
        [Column("comment_num")]
        public int CommentNum { get; set; } // 评论次数 

    }
}
