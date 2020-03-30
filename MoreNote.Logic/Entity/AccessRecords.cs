using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MoreNote.Logic.Entity
{
    public class AccessRecords
    {
        [Key]
        public long AccessId { get; set; }
        public string IP { get; set; }
        public string X_Real_IP { get; set; }
        public string X_Forwarded_For{get;set;}
        public string Referrer { get; set; }//来源 从哪个网站来的
        public string RequestHeader { get; set; }//http header
        public DateTime AccessTime { get; set; }
        public long UnixTime { get; set; }
        public long TimeInterval { get; set; }//距离上一次访问的时间间隔 如果没有上次 1
        public string url { get; set; }
    }
    public class Blacklist
    {
        [Key]
        public long IPid { get; set; }
        public string IP { get; set; }
    }
    public class BackgroudImageFile
    {
        [Key]
        public long FileId { get; set; }
        //如果 不进行FixContent处理，那么FileId=LocalFileId
        //public long LocalFileId { get;set;}//客户端首次提交文件时的客户端定义的文件ID  
        public long AlbumId { get; set; }
        public string Name { get; set; } // file name
        public string Title { get; set; } // file  name or user defind for search
        public long Size { get; set; } // file  size (byte)
        public string Type { get; set; } // file  type ""=image "doc"=word
        public string Path { get; set; } // the file path
        //0 public 1 protected 2 private
        //公开 所有人可以访问
        //保护 任何允许访问笔记的人可以允许访问
        //私有 仅允许笔记拥有者访问
        //public int AccessPermission { get;set; }

        public bool IsDefaultAlbum { get; set; }
        public DateTime CreatedTime { get; set; }

        //自定义
        public int AccessNumber { get; set; }//文件访问数 
        public string sha1 { get; set; }
        public string md5 { get; set; }
    }
}
