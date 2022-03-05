using MoreNote.Models.Enum;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreNote.Models.Entity.Leanote
{
    /// <summary>
    /// 文件仓库
    /// </summary>
    [Table("file_repository")]
    public class FileRepository
    {
        [Key]
        [Column("file_repository_id")]
        public long? FileRepositoryId { get; set; }//仓库id


        [Column("file_repository_name")]
        public long? FileRepositoryName { get; set; }//仓库唯一名称

        [Column("file_repository_summary")]
        public long? FileRepositorySummary { get; set; }//仓库摘要说明

        [Column("file_repository_license")]//开源协议
        public long? FileRepositoryLicense { get; set; }//仓库开源说明

        [Column("file_repository_type")]
        public RepositoryType RepositoryType { get; set; }//仓库类型

        [Column("owner_id")]
        public long? OwnerId { get; set; }//拥有者

        [Column("visible")]
        public bool Visible { get; set; }//是否公开仓库


    }
}
