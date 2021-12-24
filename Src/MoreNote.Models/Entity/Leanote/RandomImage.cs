using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MoreNote.Logic.Entity
{
    [Table("random_image"),Index(nameof(RandomImageId),nameof(TypeName),nameof(Sex),nameof(IsDelete),nameof(Block),nameof(FileSHA1))]
    public class RandomImage
    {
        [Key]
        [Column("random_image_id")]
        public long? RandomImageId { get; set; }
        [Column("type_name")]
        public string TypeName { get; set; }
        [Column("type_name_md5")]
        public string TypeNameMD5 { get; set; }
        [Column("type_name_sha1")]
        public string TypeNameSHA1 { get; set; }
        [Column("file_name")]
        public string FileName { get; set; }
        [Column("file_name_md5")]
        public string FileNameMD5 { get; set; }
        [Column("file_name_sha1")]
        public string FileNameSHA1 { get; set; }
        [Column("file_sha1")]
        public string FileSHA1 { get; set; }
        [Column("sex")]
        public bool Sex { get; set; }//标记这个图片含有色情信息
        [Column("block")]
        public bool Block { get; set; }//标记这个图片已经被拉黑
        [Column("is_delete")]
        public bool IsDelete { get; set; }//标记这个图片已经被删除
        [Column("is_302")]
        public bool Is302 { get;set;}//标记这个图片是否是302跳转图片
        [Column("external_link")]
        public string? ExternalLink  { get;set;}//图片的外部地址，302跳转需要用到这个值
    }
}
