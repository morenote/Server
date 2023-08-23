using Morenote.Models.Models.Entity;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreNote.Models.Entity.Leanote.Blog
{
    [Table("post")]
    public class BlogPost : BaseEntity
    {

        [Column("note_id")]
        public long? NoteId { get; set; }
        [Column("title")]
        public string Title { get; set; }
        [Column("url_title")]
        public string UrlTitle { get; set; }
        [Column("img_src")]
        public string ImgSrc { get; set; }
        [Column("created_time")]
        public DateTime CreatedTime { get; set; }
        [Column("updated_time")]
        public DateTime UpdatedTime { get; set; }
        [Column("public_time")]
        public DateTime PublicTime { get; set; }
        [Column("desc")]
        public string Desc { get; set; }
        [Column("abstract")]
        public string Abstract { get; set; }
        [Column("content")]
        public string Content { get; set; }
        [Column("tags")]
        public string[] Tags { get; set; }
        [Column("comment_num")]
        public int CommentNum { get; set; }
        [Column("read_num")]
        public int ReadNum { get; set; }
        [Column("like_num")]
        public int LikeNum { get; set; }
        [Column("is_markdown")]
        public bool IsMarkdown { get; set; }
    }
}
