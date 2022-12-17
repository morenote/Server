using Masuit.MyBlogs.Core.Models.Entity;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreNote.Models.Entity.Leanote.Synchronized
{
    /// <summary>
    /// 提交块
    /// </summary>
    public class SubmitBlock : BaseEntity
    {
        public int Version { get; set; } = 1;
        public long UserId { get; set; }//发起用户
        public DateTime Date { get; set; }
        public long Height { get; set; }//高度
        public long? PreBlockId { get; set; }//上一个提交快的哈希
        public string? PreBlockHash { get; set; }//前块哈希(可选)
        public string? SubmitHash { get; set; }//提交哈希（可选）
        public string? BlockHash { get; set; }//区块哈希
    }
}