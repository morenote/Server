using Morenote.Models.Models.Entity;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreNote.Models.Entity.Leanote.Blog
{
    [Table("user_blog_comment")]
    public class UserBlogComment : BaseEntity
    {


        [Column("can_comment")]
        public bool CanComment { get; set; } // 是否可以评论 
        [Column("comment_type")]
        public string CommentType { get; set; } // default   CommentType // leanote, or disqus
        [Column("disqus_id")]
        public string? DisqusId { get; set; }
    }
}
