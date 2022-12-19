using Morenote.Models.Models.Entity;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreNote.Models.Entity.Leanote.Synchronized
{
    [Table("submit_operation")]
    public class SubmitOperation : BaseEntity
    {
        [Column("submit_block_id")]
        public long SubmitBlockId { get; set; }
        [Column("method")]
        public OperationMethod Method { get; set; }
        [Column("target_type")]
        public TargetType TargetType { get; set; }//目标类型
        [Column("target")]
        public long? Target { get; set; }//目标id
        [Column("attributes")]
        public OperationAttributes Attributes { get; set; }
        [Column("data")]
        public string? Data { get; set; }//操作数据 json
        [Column("data_index")]
        public long? DataIndex { get; set; }//数据索引 当数据是一个非常巨大的对象（图片、附件）时，操作会指向资源索引
        [Column("data_hash")]
        public string? DateHash { get; set; }//可选 数据的哈希值
        [Column("operation_hash")]
        public string? OperationHash { get; set; }//可选 操作的哈希值
    }
}