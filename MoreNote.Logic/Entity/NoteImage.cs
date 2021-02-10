using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MoreNote.Logic.Entity
{
    [Table("note_image")]
    public class NoteImage
    {
        [Key]
        [Column("note_image_id")]
        public long? NoteImageId { get; set; } // 必须要设置bson:"_id" 
        [Column("note_id")]
        public long? NoteId { get; set; } // 笔记 
        [Column("image_id")]
        public long? ImageId { get; set; } // 图片fileId                             
        /**
         * 自定义
         * 图片引用计数器
         * 当图片引用计数器=0时，图片会被删除
         */
        [Column("use_count")]
        public int UseCount { get; set; }//图片引用计数器


    }
}
