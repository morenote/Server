using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace MoreNote.Logic.Entity
{
   public class Tag
    {
        [Key]
        public long UserId { get; set; }
        public NoteTag[] Tags { get; set; }
     

    }
    public class NoteTag
    {
        [Key]
        public long TagId { get; set; }
        public long UserId { get; set; } // 谁的 
        public string Tag { get; set; } // UserId, Tag是唯一索引
        public int Usn { get; set; } // Update Sequence Number
        public int Count { get; set; } //笔记数
        public DateTime CreatedTime { get; set; }
        public DateTime UpdatedTime { get; set; }
        public bool IsDeleted { get; set; } // 删除位 

    }
    public class TagCount
    {
        [Key]
        public long TagCountId { get; set; }
        public long UserId { get; set; } // 谁的 
        public string Tag { get; set; }
        public bool IsBlog { get; set; } // 是否是博客的tag统计 
        public int Count { get; set; } // 统计数量 
    }

}
