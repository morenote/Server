using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace MoreNote.Logic.Entity
{
   public class NoteFile
    {
        [Key]
        public long FileId { get; set; }
        public long UserId { get; set; }
        public long AlbumId { get; set; }

        public string Name { get; set; } // file name
        public string Title { get; set; } // file  name or user defind for search
        public long Size { get; set; } // file  size (byte)
        public string Type { get; set; } // file  type ""=image "doc"=word
        public string Path { get; set; } // the file path

        public bool IsDefaultAlbum { get; set; }
        public DateTime CreatedTime { get; set; }

        public long FromFileId { get; set; }//copy from fileId, for collaboration
        
        //自定义
        public int NumberOfFileReferences { get; set; }//文件引用数 当文件引用数=0 说明没有笔记引用该文件
        public string sha1 { get; set; }
        public string md5 { get; set; }
    }
}
