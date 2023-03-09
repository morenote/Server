using Morenote.Models.Models.Entity;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreNote.Models.Entity.Leanote.Blog
{
    [Table("cate")]
    public class BlogCate : BaseEntity
    {

        [Column("parent_cate_id")]
        public string ParentCateId { get; set; }
        [Column("title")]
        public string Title { get; set; }
        [Column("url_title")]
        public string UrlTitle { get; set; }
        [Column("children")]
        public BlogCate[] Children { get; set; }
    }
}
