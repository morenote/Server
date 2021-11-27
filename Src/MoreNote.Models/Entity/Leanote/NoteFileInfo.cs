using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

using MoreNote.Logic.Model;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MoreNote.Logic.Entity
{
    /// <summary>
    /// 对应Leanote的File
    /// </summary>
    [Table("note_file"),Index(nameof(FileId),nameof(UserId),nameof(SHA1))]
    public class NoteFile
    {
        [Key]
        [Column("file_id")]
        public long? FileId { get; set; }
        //如果 不进行FixContent处理，那么FileId=LocalFileId
        //public long? LocalFileId { get;set;}//客户端首次提交文件时的客户端定义的文件ID  
        [Column("user_id")]
        public long? UserId { get; set; }
        [Column("album_id")]
        public long? AlbumId { get; set; }
        [Column("name")]
        public string Name { get; set; } // file name
        [Column("title")]
        public string Title { get; set; } // file  name or user defind for search
        [Column("size")]
        public Int64 Size { get; set; } // file  size (byte)
        [Column("type")]
        public string Type { get; set; } // file  type ""=image "doc"=word
        [Column("path")]
        public string Path { get; set; } // the file path
       

        //0 public 1 protected 2 private
        //公开 所有人可以访问
        //保护 任何允许访问笔记的人可以允许访问
        //私有 仅允许笔记拥有者访问
        //public int AccessPermission { get;set; }
        [Column("is_default_album")]
        public bool IsDefaultAlbum { get; set; }
        [Column("created_time")]
        public DateTime CreatedTime { get; set; }

        [Column("from_file_id")]
        public long? FromFileId { get; set; }//copy from fileId, for collaboration

        //自定义
        [Column("number_of_file_references")]
        public int NumberOfFileReferences { get; set; }//文件引用数 当文件引用数=0 说明没有笔记引用该文件
        [Column("sha1")]
        public string SHA1 { get; set; }
        [Column("md5")]
        public string MD5 { get; set; }
        [Column("storage_type")]
        public StorageTypeEnum StorageType { get; set; } //储存方式 本地？又拍云 对象储存？
    }
}
