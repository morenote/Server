using Morenote.Models.Models.Entity;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreNote.Models.Entity.Leanote.Blog
{
    // 点赞记录
    [Table("blog_like")]
    public class BlogLike : BaseEntity
    {

        [Column("note_id")]
        public long? NoteId { get; set; }
        [Column("user_id")]
        public long? UserId { get; set; }
        [Column("created_time")]
        public DateTime CreatedTime { get; set; }

    }
}
