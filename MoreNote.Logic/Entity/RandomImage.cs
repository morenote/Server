using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MoreNote.Logic.Entity
{
    [Table("random_image")]
    public class RandomImage
    {
        [Key]
        [Column("random_image_id")]
        public long RandomImageId { get; set; }
        [Column("type_name")]
        public string TypeName { get; set; }
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
        [Column("delete")]
        public bool Delete { get; set; }//标记这个图片已经被拉黑



    }
}
