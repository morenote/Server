using Masuit.MyBlogs.Core.Models.Entity;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreNote.Models.Entity.Leanote.Synchronized
{
    public class Operation : BaseEntity
    {
        public long SubmitId { get; set; }
        public OperationMethod Method { get; set; }
        public TargetType TargetType { get; set; }//目标类型
        public long? Target { get; set; }//目标id
        public OperationAttributes Attributes { get; set; }
        public string? Date { get; set; }//操作数据 json
        public long? DataIndex { get; set; }//数据索引 当数据是一个非常巨大的对象（图片、附件）时，操作会指向资源索引
        public string? DateHash { get; set; }//可选 数据的哈希值
        public string? OperationHash { get; set; }//可选 操作的哈希值
    }
}