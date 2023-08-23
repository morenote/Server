
using Morenote.Models.Models.Entity;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MoreNote.Models.Entity.Leanote.Blog
{

    [Table("blog_info_custom")]
    // 仅仅为了博客的主题
    public class BlogInfoCustom : BaseEntity
    {



        [Column("user_id")]
        public long? UserId { get; set; }
        [Column("username")]
        public string Username { get; set; }
        [Column("user_logo")]
        public string UserLogo { get; set; }
        [Column("title")]
        public string Title { get; set; }
        [Column("sub_title")]
        public string SubTitle { get; set; }
        [Column("logo")]
        public string Logo { get; set; }
        [Column("open_comment")]
        public string OpenComment { get; set; }
        [Column("comment_type")]
        public string CommentType { get; set; }
        [Column("theme_id")]
        public string ThemeId { get; set; }
        [Column("sub_domain")]
        public string SubDomain { get; set; }
        [Column("domain")]
        public string Domain { get; set; }

    }
   

   
}
