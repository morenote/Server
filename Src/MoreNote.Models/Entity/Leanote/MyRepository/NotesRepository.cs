using Morenote.Models.Models.Entity;

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
    [Table("repository")]
    public class Repository:BaseEntity
    {
        

        [Column("name")]
        public string? Name { get; set; }//仓库唯一名称/仓库路径

        [Column("avatar")]
        public string? Avatar { get; set; }//仓库ICON图标

        [Column("star_counter")]
        public int StarCounter { get; set; }//仓库收藏数量
        [Column("fork_counter")]
        public int ForkCounter { get; set; }//仓库fork数量

        [Column("description")]
        public string? Description { get; set; }//仓库摘要说明

        [Column("license")]//开源协议
        public string? License { get; set; }//开源协议

        [Column("owner_type")]
        public RepositoryOwnerType RepositoryOwnerType { get; set; }//仓库拥有者类型
        [Column("repository_type")]
        public RepositoryType RepositoryType { get; set; }//仓库类型

        [Column("owner_id")]
        public long? OwnerId { get; set; }//拥有者

        [Column("visible")]
        public bool Visible { get; set; }//是否公开仓库

        [Column("usn")]
        public int Usn { get;set; }//仓库版本 每次提交增加+1

        [Column("create_time")]
        public DateTime CreateTime { get;set;}//仓库创建时间
        [Column("is_delete")]
        public bool IsDelete { get;set;}//仓库创建时间

        [Column("is_blog")]
        public bool IsBlog { get; set; }//仓库创建时间
        [Column("virtual_file_base_path")]
        public string? VirtualFileBasePath { get;set; }//虚拟文件基础路径 用于虚拟路径映射物理路径
        [Column("virtual_file_accessor")]
        public string? VirtualFileAccessor { get; set; }//虚拟文件访问器 用于虚拟路径映射物理路径

    }
}
