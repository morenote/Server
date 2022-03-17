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
    /// 笔记仓库
    /// </summary>
    [Table("notes_repository")]
    public class NotesRepository
    {
        [Key]
        [Column("notes_repository_id")]
        public long? NotesRepositoryId { get; set; }//仓库id

        [Column("notes_repository_name")]
        public long? NotesRepositoryName { get; set; }//仓库唯一名称

        [Column("notes_repository_summary")]
        public string? NotesRepositorySummary { get; set; }//仓库摘要说明

        [Column("notes_repository_license")]//开源协议
        public string? NotesRepositoryLicense { get; set; }//开源协议

        [Column("notes_repository_type")]
        public RepositoryType RepositoryType { get; set; }//仓库类型

        [Column("owner_id")]
        public long? OwnerId { get; set; }//拥有者

        [Column("visible")]
        public bool Visible { get; set; }//是否公开仓库

    }
}
