using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MoreNote.Logic.Entity
{
    public enum TokenEnum
    {
        TokenPwd, TokenActiveEmail, TokenUpdateEmail

    }
    [Table("tag")]
    public class Tag
    {

        [Key]
        [Column("user_id")]
        public long UserId { get; set; }
        [Column("tags")]
        public List<string> Tags { get; set; }

    }
    [Table("note_tag")]
    public class NoteTag
    {
        [Key]
        [Column("tag_id")]
        public long TagId { get; set; }
        [Column("user_id")]
        public long UserId { get; set; } // 谁的 
        [Column("tag")]
        public string Tag { get; set; } // UserId, Tag是唯一索引
        [Column("usn")]
        public int Usn { get; set; } // Update Sequence Number
        [Column("count")]
        public int Count { get; set; } //笔记数
        [Column("created_time")]
        public DateTime CreatedTime { get; set; }
        [Column("updated_time")]
        public DateTime UpdatedTime { get; set; }
        [Column("is_deleted")]
        public bool IsDeleted { get; set; } // 删除位 

    }
    [Table("tag_count")]
    public class TagCount
    {
        [Key]
        [Column("tag_count_id")]
        public long TagCountId { get; set; }
        [Column("user_id")]
        public long UserId { get; set; } // 谁的 
        [Column("tag")]
        public string Tag { get; set; }
        [Column("is_blog")]
        public bool IsBlog { get; set; } // 是否是博客的tag统计 
        [Column("tag_count")]
        public int Count { get; set; } // 统计数量 
    }

}
