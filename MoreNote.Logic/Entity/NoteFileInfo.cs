using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using MoreNote.Logic.Service.FileService;

namespace MoreNote.Logic.Entity
{
    /// <summary>
    /// 对应Leanote的File
    /// </summary>
   public class NoteFile
    {
        [Key]
        public long FileId { get; set; }
        //如果 不进行FixContent处理，那么FileId=LocalFileId
        //public long LocalFileId { get;set;}//客户端首次提交文件时的客户端定义的文件ID  
        public long UserId { get; set; }
        public long AlbumId { get; set; }

        public string Name { get; set; } // file name
        public string Title { get; set; } // file  name or user defind for search
        public Int64 Size { get; set; } // file  size (byte)
        public string Type { get; set; } // file  type ""=image "doc"=word
        public string Path { get; set; } // the file path
       // public StorageTypeEnum StorageType { get; set; } //储存方式 本地？又拍云 对象储存？

        //0 public 1 protected 2 private
        //公开 所有人可以访问
        //保护 任何允许访问笔记的人可以允许访问
        //私有 仅允许笔记拥有者访问
        //public int AccessPermission { get;set; }

        public bool IsDefaultAlbum { get; set; }
        public DateTime CreatedTime { get; set; }

        public long FromFileId { get; set; }//copy from fileId, for collaboration
        
        //自定义
        public int NumberOfFileReferences { get; set; }//文件引用数 当文件引用数=0 说明没有笔记引用该文件
        public string SHA1 { get; set; }
        public string MD5 { get; set; }
    }
}
