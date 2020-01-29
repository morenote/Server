using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace MoreNote.Logic.Entity
{
    public class NoteImage
    {
        [Key]
        public long NoteImageId { get; set; } // 必须要设置bson:"_id" 
        public long NoteId { get; set; } // 笔记 
        public long ImageId { get; set; } // 图片fileId 
        //自定义
        /**
         * 图片引用计数器
         * 当图片引用计数器=0时，图片会被删除
         */
        public int userCount { get; set; }//图片引用计数器


    }
}
