using Morenote.Models.Models.Entity;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreNote.Models.Entity.Leanote.Synchronized
{
    /// <summary>
    /// 提交块
    /// </summary>
    [Table("submit_block")]
    public class SubmitBlock : BaseEntity
    {
        [Column("version")]
        public int Version { get; set; } = 1;
        [Column("user_id")]
        public long? UserId { get; set; }//发起用户
        [Column("tree_id")]
        public long? TreeId { get; set; }//submitTreeId
        [Column("date")]
        public DateTime Date { get; set; }
        [Column("height")]
        public int Height { get; set; }//高度
        [Column("pre_block_id")]
        public long? PreBlockId { get; set; }//上一个提交快的Id
        [Column("pre_block_hash")]
        public string? PreBlockHash { get; set; }//前块哈希(可选)
        [Column("submit_hash")]
        public string? SubmitHash { get; set; }//提交哈希（可选）
        [Column("block_hash")]
        public string? BlockHash { get; set; }//区块哈希
    }
}