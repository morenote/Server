using Morenote.Models.Models.Entity;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreNote.Models.Entity.Leanote.Blog
{
    [Table("blog_comment_public")]
    public class BlogCommentPublic : BaseEntity
    {

        [Column("blog_comment")]
        BlogComment BlogComment { get; set; }
        [Column("is_i_like_it")]
        public bool IsILikeIt { get; set; }
    }
}
