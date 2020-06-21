using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MoreNote.Logic.Entity
{
    public class RandomImage
    {
        [Key]
        public long RandomImageId { get; set; }
        public string TypeName { get; set; }
        public string TypeNameMD5 { get; set; }
        public string TypeNameSHA1 { get; set; }
        public string FileName { get; set; }
        public string FileNameMD5 { get; set; }
        public string FileNameSHA1 { get; set; }
        public string FileSHA1 { get; set; }
        public bool Sex { get; set; }//标记这个图片含有色情信息

        public bool Block { get; set; }//标记这个图片已经被拉黑
        public bool Delete { get; set; }//标记这个图片已经被拉黑



    }
}
