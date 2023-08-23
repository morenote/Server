using Morenote.Models.Models.Entity;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreNote.Models.Entity.Leanote.Blog
{
    //// 单页
    [Table("blog_single")]
    public class BlogSingle : BaseEntity
    {

        [Column("user_id")]
        public long? UserId { get; set; }
        [Column("title")]
        public string Title { get; set; }
        [Column("url_title")]
        public string UrlTitle { get; set; }// 2014/11/11
        [Column("content")]
        public string Content { get; set; }
        [Column("updated_time")]
        public DateTime UpdatedTime { get; set; }
        [Column("created_time")]
        public DateTime CreatedTime { get; set; }


    }
}
