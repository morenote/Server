using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Morenote.Models.Models.Entity;

namespace MoreNote.Models.Entity.Leanote.Notes
{
    [Table("note_image")]
    public class NoteImage : BaseEntity
    {

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
