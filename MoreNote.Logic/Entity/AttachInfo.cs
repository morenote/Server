
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using MoreNote.Logic.Service.FileService;

namespace MoreNote.Logic.Entity
{
  
    /// <summary>
    /// 附件
    /// </summary>
    public class AttachInfo
    {
        [Key]
        public long AttachId{ get; set; }
        public long UserId { get;set;}
        public long NoteId{ get; set; }
        public long UploadUserId{ get; set; } // 可以不是note owner, 协作者userId
        public string Name{ get; set; } // file name, MD5, such as 13232312.doc
        public string Title{ get; set; } // raw file name
        public Int64 Size{ get; set; } // file size (byte)
        public string Type{ get; set; }   // file type, "doc" = word
        public string Path{ get; set; }  // the file path such as: files/userId/attachs/adfadf.doc
       // public  StorageTypeEnum StorageType { get; set; }//附件储存方式
        
        public DateTime CreatedTime{ get; set; }


    }
}
